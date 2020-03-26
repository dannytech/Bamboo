using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Bamboo.Protocol
{
    interface IWritable
    {
        public int Position { get; set; }
        public DataWriter Writer { get; }
        public void Write(byte[] bytes);
    }

    interface ISerializable
    {
        public Dictionary<string, object> ToSerializable();
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

        public void WriteJson(object value)
        {
            string json = JsonSerializer.Serialize(value); // Convert to JSON representation

            WriteVarChar(json);
        }

        public void WriteChar(char value)
        {
            // Read two bytes from the stream
            Write(BitConverter.GetBytes(value));
        }

        public void WriteUInt16(ushort value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void WriteUInt32(uint value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void WriteUInt64(ulong value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void WriteInt16(short value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void WriteInt32(int value)
        {
            Write(BitConverter.GetBytes(value));
        }

        public void WriteInt64(long value)
        {
            Write(BitConverter.GetBytes(value));
        }
    }
}
