// See https://aka.ms/new-console-template for more information

using System.IO.Pipes;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

const string pipeName = "test";
await using var server = new NamedPipeServerStream(pipeName);
await using var client = new NamedPipeClientStream(pipeName);
await client.ConnectAsync();

await server.WaitForConnectionAsync();

var message = new byte[] { 4, 3, 2, 1 };

var hasher = new SH1Hasher();

var writer = new MessageWriter(hasher);
await writer.Write(client, message);

var reader = new MessageReader(hasher);
var receivedMessage = await reader.Read(server);

Console.ReadLine();

interface IPayloadHasher
{
    byte[] Hash(byte[] payload);
}

class SH1Hasher : IPayloadHasher, IDisposable
{
    private readonly SHA1 _hasher;
    public SH1Hasher()
    {
        _hasher = SHA1.Create();
    }
    public byte[] Hash(byte[] payload)
    {
        return _hasher.ComputeHash(payload);
    }

    public void Dispose()
    {
        _hasher.Dispose();
    }
}

class MessageReader(IPayloadHasher hasher)
{
    private readonly IPayloadHasher _hasher = hasher;

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

class MessageWriter(IPayloadHasher hasher)
{
    private readonly IPayloadHasher _hasher = hasher;
    
    public async Task Write(PipeStream stream, byte[] payload)
    {
        await WriteHeader(stream, payload);
        await stream.WriteAsync(payload);
    }

    private async Task WriteHeader(PipeStream stream, byte[] payload)
    {
        var header = new IPCMessageHeader
        {
            Id = Guid.NewGuid(),
            PayloadSize = payload.Length
        };

        ComputeHash(payload, ref header);

        var headers = new IPCMessageHeader[] { header };
        var buffer = MemoryMarshal.Cast<IPCMessageHeader, byte>(headers).ToArray();
        await stream.WriteAsync(buffer);
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

[StructLayout(LayoutKind.Sequential)]
unsafe struct IPCMessageHeader
{
    public Guid Id;
    public Compression Compression;
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