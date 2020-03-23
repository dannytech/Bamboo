using Bamboo.Protocol;
using System.Collections.Generic;

namespace Bamboo.Game.Chat
{
    class ChatComponentClickEventAction
    {
        public static string OpenURL { get; } = "open_url";
        public static string RunCommand { get; } = "run_command";
        public static string SuggestCommand { get; } = "suggest_command";
        public static string ChangePage { get; } = "change_page";
    }

    class ChatComponentClickEvent : ISerializable
    {
        public string Action { get; set; }
        public string Value { get; set; }

        public ChatComponentClickEvent(string action, string value)
        {
            Action = action;
            Value = value;
        }

        public Dictionary<string, object> ToSerializable()
        {
            Dictionary<string, object> root = new Dictionary<string, object>
            {
                { "action", Action },
                { "value", Value }
            };

            return root;
        }
    }
}
