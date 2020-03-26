namespace Bamboo.Game.Chat
{
    class ChatComponentStyle
    {
        public static int None { get; } = 0b00000;
        public static int Bold { get; } = 0b00001;
        public static int Italic { get; } = 0b00010;
        public static int Underlined { get; } = 0b00100;
        public static int Strikethrough { get; } = 0b01000;
        public static int Obfuscated { get; } = 0b10000;
    }

    class ChatComponentColor
    {
        public static string Black { get; } = "black";
        public static string DarkBlue { get; } = "dark_blue";
        public static string DarkGreen { get; } = "dark_green";
        public static string DarkCyan { get; } = "dark_aqua";
        public static string DarkRed { get; } = "dark_red";
        public static string Purple { get; } = "dark_purple";
        public static string Gold { get; } = "gold";
        public static string Gray { get; } = "gray";
        public static string DarkGray { get; } = "dark_gray";
        public static string Blue { get; } = "blue";
        public static string BrightGreen { get; } = "green";
        public static string Cyan { get; } = "aqua";
        public static string Red { get; } = "red";
        public static string Pink { get; } = "light_purple";
        public static string Yellow { get; } = "yellow";
        public static string White { get; } = "white";
    }

}