namespace Bamboo.Game.Chat
{
    class ChatComponentClickEventAction
    {
        public static string OpenURL { get; } = "open_url";
        public static string RunCommand { get; } = "run_command";
        public static string SuggestCommand { get; } = "suggest_command";
        public static string ChangePage { get; } = "change_page";
    }

    class ChatComponentClickEvent
    {
        public string Action { get; set; }
        public string Value { get; set; }

        public ChatComponentClickEvent(string action, string value)
        {
            Action = action;
            Value = value;
        }
    }
}
