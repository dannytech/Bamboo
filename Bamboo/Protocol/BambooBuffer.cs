using System.Collections.Generic;

namespace Bamboo.Protocol
{
    class BambooBuffer : IReadable, IWritable
    {
        private int Cursor = 0;
        public readonly List<byte> Buffer;
        public int Length = 0;

        public BambooBuffer()
        {
            Buffer = new List<byte>();
        }

        public BambooBuffer(byte[] bytes)
        {
            Buffer = new List<byte>(bytes);
            Length = Buffer.Count;
        }

        public byte[] Read(int length)
        {
            // Write the data into a new buffer
            byte[] bytes = Buffer.GetRange(Cursor, length).ToArray();
            Cursor += length;

            return bytes;
        }

        public void Write(byte[] bytes)
        {
            Buffer.InsertRange(Cursor, bytes);
            Cursor += bytes.Length;
            Length += bytes.Length;
        }

        public byte ReadByte()
        {
            return Read(1)[0];
        }

        public void WriteByte(byte value)
        {
            Write(new byte[] { value });
        }
    }
}
