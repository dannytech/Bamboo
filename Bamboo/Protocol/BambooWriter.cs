using System;
using System.Text;

namespace Bamboo.Protocol
{
    class BambooWriter
    {
        private readonly IWritable Writable;

        public BambooWriter(IWritable writable)
        {
            Writable = writable;
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

                Writable.WriteByte(result);
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

                Writable.WriteByte(result);
            } while (temp != 0);
        }

        public void WriteVarChar(string value)
        {
            WriteVarInt(value.Length);
            Writable.Write(Encoding.UTF8.GetBytes(value));
        }

        public void WriteInt64(long value)
        {
            Writable.Write(BitConverter.GetBytes(value));
        }
    }
}
