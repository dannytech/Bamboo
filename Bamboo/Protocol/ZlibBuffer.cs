using System;
using System.IO;
using System.IO.Compression;

namespace Bamboo.Protocol
{
    class ZlibBuffer : IReadable, IWritable
    {
        private readonly CompressionMode _Mode;
        private readonly DataBuffer _Buffer;

        public int Length { get => _Buffer.Length; }
        public int Position { get => _Buffer.Position; set => _Buffer.Position = value; }
        public DataReader Reader { get; }
        public DataWriter Writer { get; }

        public ZlibBuffer(CompressionMode mode) : this(new byte[] { }, mode) { }
        public ZlibBuffer(byte[] bytes, CompressionMode mode)
        {
            _Mode = mode;
            _Buffer = new DataBuffer(bytes);

            Reader = new DataReader(this);
            Writer = new DataWriter(this);
        }

        // Compress Mode: Write uncompressed, Store uncompressed, Read compressed counting uncompressed bytes (+headers)
        // Decompress Mode: Write compressed, Store uncompressed, Read uncompressed counting uncompressed bytes

        private byte[] Deflate(byte[] bytes, CompressionMode mode)
        {
            MemoryStream source = new MemoryStream(bytes);
            MemoryStream target = new MemoryStream();
            DeflateStream zlib = new DeflateStream(target, mode);

            source.CopyTo(zlib); // Performs the actual compression
            zlib.Close();

            return target.ToArray();
        }

        private byte[] Zlib()
        {
            return new byte[] { 0x78, 0x01 };
        }

        private uint Adler32(byte[] bytes)
        {
            const int mod = 65521;
            uint a = 1, b = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                a = (a + bytes[i]) % mod;
                b = (b + a) % mod;
            }

            // 4 byte checksum
            return (b * 65536) + a;
        }

        public byte[] Read()
        {
            return Read(Length - Position);
        }
        public byte[] Read(int length)
        {
            byte[] bytes = _Buffer.Read(length);

            // Read the bytes from the stream
            if (_Mode == CompressionMode.Compress)
            {
                bytes = Deflate(bytes, CompressionMode.Compress);
                DataBuffer buffer = new DataBuffer(bytes);

                // Prepend a zlib header
                buffer.Writer.Write(Zlib());

                // Append an Adler32 checksum for the requested data
                buffer.Position = buffer.Length - 1;
                buffer.Writer.WriteUInt32(Adler32(bytes));

                bytes = buffer.Reader.ReadAll();
            }

            return bytes;
        }

        public void Write(byte[] bytes)
        {
            if (_Mode == CompressionMode.Decompress)
            {
                DataBuffer buffer = new DataBuffer(bytes);

                // Confirm and remove header
                if (buffer.Reader.Read(2).AsSpan().SequenceEqual(Zlib().AsSpan()))
                {
                    // Get the compressed bytes
                    byte[] compressed = buffer.Reader.Read(buffer.Length - 6); // Ignoring 2-byte header already read and 4-byte checksum
                    uint checksum = buffer.Reader.ReadUInt32();

                    // Decompress the bytes before storing them
                    bytes = Deflate(compressed, CompressionMode.Decompress);

                    // Verify the checksum
                    if (Adler32(bytes) != checksum)
                    {
                        Console.WriteLine("Checksum check failed on compressed packet");
                    }
                }
            }

            _Buffer.Write(bytes);
        }
    }
}
