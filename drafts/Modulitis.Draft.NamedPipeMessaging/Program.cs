// See https://aka.ms/new-console-template for more information

using System.IO.Pipes;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

const string pipeName = "test";
await using var server = new NamedPipeServerStream(pipeName);
await using var client = new NamedPipeClientStream(pipeName);
await client.ConnectAsync();

await server.WaitForConnectionAsync();

var typedMessage = new Message(Guid.Parse("F4692F36-E291-4108-B64C-3CE4D00AD5D6"), "test");
var messageBytes = new byte[] { 123,34,73,100,34,58,34,102,52,54,57,50,102,51,54,45,101,50,57,49,45,52,49,48,56,45,98,54,52,99,45,51,99,101,52,100,48,48,97,100,53,100,54,34,44,34,84,101,120,116,34,58,34,116,101,115,116,34,125 };

var hasher = new Sh1Hasher();
var serializer = new JsonPayloadSerializer();

await using var processClient = new ProcessClient(client);
var messageSender = new MessageSender(serializer, hasher, processClient);
await messageSender.SendAsync(typedMessage, new CancellationToken());

var reader = new MessageReader(hasher);
var receivedMessage = await reader.Read(server);

var isSameSentAndReceived = messageBytes.SequenceEqual(receivedMessage.Payload);

Console.ReadLine();

interface IProcessServer
{
    
}

class Message(Guid id, string text)
{
    public Guid Id { get; } = id;
    public string Text { get; } = text;
}

interface IProcessClient: IAsyncDisposable
{
    Task SendAsync(byte[] bytes, CancellationToken token);
}

class ProcessClient : IProcessClient
{
    private readonly PipeStream _clientStream;

    public ProcessClient(PipeStream clientStream)
    {
        ArgumentNullException.ThrowIfNull(clientStream);
        _clientStream = clientStream;
    }
    
    public async Task SendAsync(byte[] bytes, CancellationToken token)
    {
        await _clientStream.WriteAsync(bytes, token);
    }

    public async ValueTask DisposeAsync()
    {
        await _clientStream.DisposeAsync();
    }
}

interface IMessageDispatcher
{
    Task Dispatch<T>(T message);
}

interface IMessageSender
{
    Task SendAsync<T>(T message, CancellationToken token);
}

class MessageSender(IPayloadSerializer _serializer, IPayloadHasher _hasher, IProcessClient _client) : IMessageSender
{
    public async Task SendAsync<T>(T message, CancellationToken token)
    {
        var messageWriter = new MessageWriter();
        var payload = Payload.Create(message, _serializer, _hasher);
        await messageWriter.Write(_client, payload, token);
    }
}

interface IPayloadSerializer
{
    byte[] Serialize<T>(T message);
    T? Deserialize<T>(byte[] bytes);
}

class JsonPayloadSerializer : IPayloadSerializer
{
    public byte[] Serialize<T>(T message)
    {
        return JsonSerializer.SerializeToUtf8Bytes(message);
    }

    public T Deserialize<T>(byte[] bytes)
    {
        var deserialized = JsonSerializer.Deserialize<T>(bytes);
        if (deserialized is null)
            throw new Exception();
        return deserialized;
    }
}

interface IPayloadHasher: IDisposable
{
    byte[] Hash(byte[] payload);
}

class Sh1Hasher : IPayloadHasher
{
    private readonly SHA1 _hasher = SHA1.Create();

    public byte[] Hash(byte[] payload)
    {
        return _hasher.ComputeHash(payload);
    }

    public void Dispose()
    {
        _hasher.Dispose();
    }
}

class MessageReader(IPayloadHasher _hasher)
{
    public async Task<IPCMessage> Read(PipeStream stream)
    {
        var header = await ReadHeader(stream);

        var buffer = new byte[header.PayloadHeader.Size];
        var result = await stream.ReadAsync(buffer);
        if (result != buffer.Length)
            throw new Exception();

        if (!CheckHash(header, buffer))
            throw new Exception();

        var bufferCopy = new byte[result];
        Buffer.BlockCopy(buffer, 0, bufferCopy, 0, result);

        return new IPCMessage(header, buffer);
    }

    private async Task<IPCMessageHeader> ReadHeader(PipeStream stream)
    {
        var buffer = new byte[IPCMessageHeader.Size];
        var result = await stream.ReadAsync(buffer);
        if (result != buffer.Length)
            throw new Exception();
                    
        var headers = MemoryMarshal.Cast<byte, IPCMessageHeader>(buffer).ToArray();
        return headers[0];
    }

    private unsafe bool CheckHash(in IPCMessageHeader header, byte[] payload)
    {
        var computedHash = _hasher.Hash(payload);
        fixed (byte* fixedPayloadHash = header.PayloadHeader.Hash)
        {
            var payloadHash = new Span<byte>(fixedPayloadHash, PayloadHeader.PayloadHashSize);
            return payloadHash.SequenceEqual(computedHash);
        }
    }
}

class MessageWriter
{
    public async Task Write(IProcessClient client, Payload payload, CancellationToken token)
    {
        await WriteHeader(client, payload, token);
        await client.SendAsync(payload.Data, token);
    }

    private async Task WriteHeader(IProcessClient client, Payload payload, CancellationToken token)
    {
        var header = new IPCMessageHeader
        {
            Id = Guid.NewGuid(),
            PayloadHeader = payload.Header
        };

        var headers = new IPCMessageHeader[] { header };
        var buffer = MemoryMarshal.Cast<IPCMessageHeader, byte>(headers).ToArray();
        await client.SendAsync(buffer, token);
    }
}

enum Compression: byte
{
    None
}

enum MessageType : byte
{
    Data
}

enum MessageFlag : byte
{
    None,
    Ack
}

[StructLayout(LayoutKind.Sequential)]
unsafe struct PayloadHeader
{
    private const int MaxClassNameLength = 512;
    public const int PayloadHashSize = 20;
    
    public int Size;
    public fixed char Type[MaxClassNameLength];
    public fixed byte Hash[PayloadHashSize];

    public static PayloadHeader Create()
        => new PayloadHeader();
    
    public void Write(ReadOnlySpan<char> type, byte[] data, IPayloadHasher hasher)
    {
        Clean();
        
        var computedHash = hasher.Hash(data);
        
        fixed (char* fixedPayloadTypeNamePtr = type)
        fixed (char* fixedPayloadTypePtr = Type)
        {
            NativeMemory.Copy(fixedPayloadTypeNamePtr, fixedPayloadTypePtr, (uint)type.Length * sizeof(char));
        }

        fixed (byte* computedHashPtr = computedHash)
        fixed (byte* fixedPayloadHashPtr = Hash)
        {
            NativeMemory.Copy(computedHashPtr, fixedPayloadHashPtr, PayloadHashSize);
        }

        Size = data.Length;
    }
    
    public void Clean()
    {
        Size = 0;
        fixed (char* fixedPayloadTypePtr = Type)
        fixed (byte* fixedPayloadHashPtr = Hash)
        {
            NativeMemory.Fill(fixedPayloadTypePtr, MaxClassNameLength, 0);
            NativeMemory.Fill(fixedPayloadHashPtr, PayloadHashSize, 0);
        }
    }
}

class Payload
{
    private Payload(byte[] data)
    {
        Data = data;
    }

    public PayloadHeader Header { get; private set; }
    public byte[] Data { get; private set; }
    
    public static Payload Create<T>(T @object, IPayloadSerializer serializer, IPayloadHasher hasher)
    {
        var payloadTypeName = typeof(T).Name;
        var serializedPayload = serializer.Serialize(@object);

        var payload = new Payload(serializedPayload);

        var header = PayloadHeader.Create();
        header.Write(payloadTypeName, serializedPayload, hasher);
        payload.Header = header;

        return payload;
    }
}

[StructLayout(LayoutKind.Sequential)]
unsafe struct IPCMessageHeader
{
    public Guid Id;
    public Compression Compression;
    public MessageType Type;
    public MessageFlag Flag;
    public PayloadHeader PayloadHeader;
    public static int Size => sizeof(IPCMessageHeader);
}

class IPCMessage
{
    public IPCMessage(IPCMessageHeader type, byte[] payload)
    {
        Type = type;
        Payload = payload;
    }
    public IPCMessageHeader Type { get; }
    public byte[] Payload { get; }
}