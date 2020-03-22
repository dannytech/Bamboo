using System.IO;

namespace Bamboo.Protocol
{
    class DataStream : IReadable, IWritable
    {
        private readonly Stream _Stream;

        public int Length {
            get => (int)_Stream.Length;
        }
        public int Position
        {
            get => (int)_Stream.Position;
            set => _Stream.Position = value;
        }
        public DataReader Reader { get; }
        public DataWriter Writer { get; }

        public DataStream(Stream stream)
        {
            _Stream = stream;

            Reader = new DataReader(this);
            Writer = new DataWriter(this);
        }

        public byte[] Read()
        {
            return Read((int)(_Stream.Length - _Stream.Position));
        }
        public byte[] Read(int length)
        {
            // Read bytes into a buffer which we will return directly
            byte[] bytes = new byte[length];
            _Stream.Read(bytes, 0, length);

            return bytes;
        }

        public void Write(byte[] bytes)
        {
            _Stream.Write(bytes, 0, bytes.Length);
        }
    }
}