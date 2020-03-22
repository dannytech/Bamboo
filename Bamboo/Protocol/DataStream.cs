using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Bamboo.Protocol
{
    interface IReadable
    {
        public byte[] Read(int length);
    }

    interface IWritable
    {
        public void Write(byte[] bytes);
    }

    class DataStream : IReadable, IWritable
    {
        private readonly Stream Stream;

        public DataStream(Stream stream)
        {
            Stream = stream;
        }

        public byte[] Read(int length)
        {
            // Read bytes into a buffer which we will return directly
            byte[] bytes = new byte[length];
            Stream.Read(bytes, 0, length);

            return bytes;
        }

        public void Write(byte[] bytes)
        {
            Stream.Write(bytes, 0, bytes.Length);
        }
    }
}