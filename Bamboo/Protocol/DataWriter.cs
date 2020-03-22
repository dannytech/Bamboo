using System;
using System.Text;

namespace Bamboo.Protocol
{
    interface IWritable
    {
        public int Position { get; set; }
        public DataWriter Writer { get; }
        public void Write(byte[] bytes);
    }

    class DataWriter
    {
        private readonly IWritable Writable;

        public DataWriter(IWritable writable)
        {
            Writable = writable;
        }

        public void Write(byte[] bytes)
        {
            Writable.Write(bytes);
        }

        public void WriteByte(byte value)
        {
            Write(new byte[] { value });
        }

        public void WriteVarInt(int value)
        {
            uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(value)); // This is needed to force a bitwise shift rather than an arithmetic shift

            do
            {
                byte result = (byte)(temp & 0b01111111);

                temp >>= 7;
                if (temp != 0)
                    result |= 0b10000000;

                WriteByte(result);
            } while (temp != 0);
        }

        public void WriteVarLong(long value)
        {
            ulong temp = BitConverter.ToUInt64(BitConverter.GetBytes(value)); // This is needed to force a bitwise shift rather than an arithmetic shift

            do
            {
                byte result = (byte)(temp & 0b01111111);

                temp >>= 7;
                if (temp != 0)
                    result |= 0b10000000;

                WriteByte(result);
            } while (temp != 0);
        }

        public void WriteVarChar(string value)
        {
            WriteVarInt(value.Length);
            Write(Encoding.UTF8.GetBytes(value));
        }

        public void WriteInt64(long value)
        {
            Write(BitConverter.GetBytes(value));
        }
    }
}
