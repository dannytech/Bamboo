using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Bamboo.Protocol
{
    class DataStream : IReadable, IWritable
    {
        private readonly Stream _Stream;

        public DataStream(Stream stream)
        {
            _Stream = stream;
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