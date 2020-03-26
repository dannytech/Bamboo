using System;

namespace Bamboo.Protocol
{
    class StringBuffer
    {
        private readonly string _String;

        public int Length { get => _String.Length; }
        public int Position { get; set; }
        public int Available { get => Length - Position; }

        public StringBuffer(string str)
        {
            _String = str;
            Position = 0;
        }

        public string Read()
        {
            string value = _String.Substring(Position, Available);
            Position = Length;

            return value;
        }

        public char ReadChar()
        {
            return _String[Position++];
        }
    }
}
