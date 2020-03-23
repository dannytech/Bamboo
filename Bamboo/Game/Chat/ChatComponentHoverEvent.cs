namespace Bamboo.Game.Chat
{
    class ChatComponentHoverEventAction
    {
        public static string ShowText { get; } = "show_text";
        public static string ShowItem { get; } = "show_item";
        public static string ShowEntity { get; } = "show_entity";
    }

    class ChatComponentHoverEvent
    {
        public ChatComponentHoverEventAction Action { get; set; }
        public string Value { get; set; }

        public ChatComponentHoverEvent(ChatComponentHoverEventAction action, string value)
        {
            Action = action;
            Value = value;
        }
    }
}
