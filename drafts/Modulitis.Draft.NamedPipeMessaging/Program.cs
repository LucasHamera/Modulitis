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
var serializer = new JsonMessageSerializer();

await using var processClient = new ProcessClient(client);
var messageSender = new MessageSender(serializer, hasher, processClient);
await messageSender.SendAsync(typedMessage, new CancellationToken());

var reader = new MessageReader(hasher);
var receivedMessage = await reader.Read(server);

var isSameSentAndReceived = messageBytes.SequenceEqual(receivedMessage.Payload);

Console.ReadLine();

class Message
{
    public Message(Guid id, string text)
    {
        Id = id;
        Text = text;
    }
    public Guid Id { get; }
    public string Text { get; }
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

class MessageSender(IMessageSerializer _messageSerializer, IPayloadHasher _hasher, IProcessClient _client) : IMessageSender
{
    public async Task SendAsync<T>(T message, CancellationToken token)
    {
        var serializedMessage = _messageSerializer.Serialize(message);
        var messageWriter = new MessageWriter(_hasher);
        await messageWriter.Write(_client, serializedMessage, token);
    }
}

interface IMessageSerializer
{
    byte[] Serialize<T>(T message);
    T? Deserialize<T>(byte[] bytes);
}

class JsonMessageSerializer : IMessageSerializer
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

        var buffer = new byte[header.PayloadSize];
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
        fixed (byte* fixedPayloadHash = header.PayloadHash)
        {
            var payloadHash = new Span<byte>(fixedPayloadHash, IPCMessageHeader.PayloadHashSize);
            return payloadHash.SequenceEqual(computedHash);
        }
    }
}

class MessageWriter(IPayloadHasher _hasher)
{
    public async Task Write(IProcessClient client, byte[] payload, CancellationToken token)
    {
        await WriteHeader(client, payload, token);
        await client.SendAsync(payload, token);
    }

    private async Task WriteHeader(IProcessClient client, byte[] payload, CancellationToken token)
    {
        var header = new IPCMessageHeader
        {
            Id = Guid.NewGuid(),
            PayloadSize = payload.Length
        };

        ComputeHash(payload, ref header);

        var headers = new IPCMessageHeader[] { header };
        var buffer = MemoryMarshal.Cast<IPCMessageHeader, byte>(headers).ToArray();
        await client.SendAsync(buffer, token);
    }

    private unsafe void ComputeHash(byte[] payload, ref IPCMessageHeader header)
    {
        var computedHash = _hasher.Hash(payload);
        fixed (byte* fixedPayloadHash = header.PayloadHash)
        {
            Marshal.Copy(computedHash,0, new IntPtr(fixedPayloadHash), computedHash.Length);
        }
    }
}

enum Compression: byte
{
    None
}

enum MessageType : byte
{
    Data,
    Ack
}

[StructLayout(LayoutKind.Sequential)]
unsafe struct IPCMessageHeader
{
    public Guid Id;
    public Compression Compression;
    public MessageType Type;
    public int PayloadSize;
    public fixed byte PayloadHash[PayloadHashSize];
    public const int PayloadHashSize = 20;
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