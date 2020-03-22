using System;
using System.IO;
using System.Text;

namespace Bamboo.Protocol
{
    class BambooReader
    {
        private readonly IReadable Readable;

        public BambooReader(IReadable readable)
        {
            Readable = readable;
        }

        public byte[] Read(int length)
        {
            return Readable.Read(length);
        }

        public byte ReadByte()
        {
            return Read(1)[0];
        }

        public bool ReadBool()
        {
            return ReadByte() != 0x00;
        }

        public int ReadVarInt()
        {
            int result = 0;
            int size = 0;
            int b;

            do
            {
                // Read a single byte at a time
                b = ReadByte();

                // Perform logical operations to read a variable-length number using the new byte
                int value = b & 0b01111111;
                result |= (value << (7 * size++));

                if (size > 5)
                {
                    throw new IOException("VarInt is too big");
                }
            } while ((b & 0b10000000) != 0);

            return result;
        }

        public long ReadVarLong()
        {
            long result = 0;
            int size = 0;
            int b;

            do
            {
                // Read a single byte at a time
                b = ReadByte();

                // Perform logical operations to read a variable-length number using the new byte
                long value = b & 0b01111111;
                result |= (value << (7 * size++));

                if (size > 10)
                {
                    throw new IOException("VarLong is too big");
                }
            } while ((b & 0b10000000) != 0);

            return result;
        }

        public string ReadVarChar()
        {
            // Get the character count
            int length = ReadVarInt();

            // Read the specified amount of string bytes
            byte[] chars = Read(length);

            // Convert the bytes to a string
            return Encoding.Default.GetString(chars);
        }

        public ushort ReadUInt16()
        {
            // Read two bytes from the stream
            return BitConverter.ToUInt16(Read(2));
        }

        public short ReadInt16()
        {
            // Read two bytes from the stream
            return BitConverter.ToInt16(Read(2));
        }

        public uint ReadUInt32()
        {
            // Read four bytes from the stream
            return BitConverter.ToUInt32(Read(4));
        }

        public int ReadInt32()
        {
            // Read four bytes from the stream
            return BitConverter.ToInt32(Read(4));
        }

        public long ReadInt64()
        {
            // Read eight bytes from the stream
            return BitConverter.ToInt64(Read(8));
        }

        public float ReadSingle()
        {
            // Read four bytes from the stream
            return BitConverter.ToSingle(Read(4));
        }

        public double ReadDouble()
        {
            // Read eight bytes from the stream
            return BitConverter.ToDouble(Read(8));
        }
    }
}
