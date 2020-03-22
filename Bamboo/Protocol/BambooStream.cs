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
        public byte ReadByte();
    }

    interface IWritable
    {
        public void Write(byte[] bytes);
        public void WriteByte(byte value);
    }

    class BambooStream : IReadable, IWritable
    {
        private readonly Stream Stream;

        public BambooStream(Stream stream)
        {
            Stream = stream;
        }

        public byte[] Read(int length/*, bool forceBE = false*/)
        {
            // Read bytes into a buffer which we will return directly
            byte[] bytes = new byte[length];
            Stream.Read(bytes, 0, length);
            Console.WriteLine(BitConverter.ToString(bytes));

            // All types except VarInt, VarLong, and VarChar are big-endian, so on little-endian systems we need to flip the buffers
            //if (BitConverter.IsLittleEndian && forceBE)
            //    Array.Reverse(bytes);

            return bytes;
        }

        public void Write(byte[] bytes/*, bool forceLE = false*/)
        {
            // If the current system is little-endian, we have to convert to big-endian before sending
            //if (BitConverter.IsLittleEndian && !forceLE)
            //    Array.Reverse(bytes);

            Stream.Write(bytes, 0, bytes.Length);
        }

        public byte ReadByte()
        {
            // Read 1 byte into a 1-byte buffer and then return just that byte
            return Read(1)[0];
        }

        public void WriteByte(byte payload)
        {
            Write(new byte[] { payload });
        }
    }
}