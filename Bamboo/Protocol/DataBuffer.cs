using System.Collections.Generic;

namespace Bamboo.Protocol
{
    class DataBuffer : IReadable, IWritable
    {
        private readonly List<byte> _Buffer;

        public int Length { get => _Buffer.Count; }
        public int Position { get; set; }
        public int Available { get => Length - Position; }
        public DataReader Reader { get; }
        public DataWriter Writer { get; }

        public DataBuffer() : this(new byte[] { }) { }
        public DataBuffer(byte[] bytes)
        {
            _Buffer = new List<byte>(bytes);

            Position = 0;
            Reader = new DataReader(this);
            Writer = new DataWriter(this);
        }

        public byte[] Read()
        {
            return Read(Available);
        }
        public byte[] Read(int length)
        {
            // Write the data into a new buffer
            byte[] bytes = _Buffer.GetRange(Position, length).ToArray();
            Position += length;

            return bytes;
        }

        public void Write(byte[] bytes)
        {
            _Buffer.InsertRange(Position, bytes);
            Position += bytes.Length;
        }
    }
}
