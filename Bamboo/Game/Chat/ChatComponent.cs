using System;
using System.Collections.Generic;

namespace Bamboo.Game.Chat
{
    class ChatStyle
    {
        public static byte None { get; } = 0b00000; // TODO Implement Reset inheritance
        public static byte Bold { get; } = 0b00001;
        public static byte Italic { get; } = 0b00010;
        public static byte Underlined { get; } = 0b00100;
        public static byte Strikethrough { get; } = 0b01000;
        public static byte Obfuscated { get; } = 0b10000;
    }

    class ChatColor
    {
        public static char Black { get; } = '0';
        public static char DarkBlue { get; } = '1';
        public static char DarkGreen { get; } = '2';
        public static char DarkCyan { get; } = '3';
        public static char DarkRed { get; } = '4';
        public static char Purple { get; } = '5';
        public static char Gold { get; } = '6';
        public static char Gray { get; } = '7';
        public static char DarkGray { get; } = '8';
        public static char Blue { get; } = '9';
        public static char BrightGreen { get; } = 'a';
        public static char Cyan { get; } = 'b';
        public static char Red { get; } = 'c';
        public static char Pink { get; } = 'd';
        public static char Yellow { get; } = 'e';
        public static char White { get; } = 'f';
    }

    class ChatClickEventAction
    {
        public string OpenURL { get; } = "open_url";
        public string RunCommand { get; } = "run_command";
        public string SuggestCommand { get; } = "suggest_command";
        public string ChangePage { get; } = "change_page";
    }

    class ChatHoverEventAction
    {
        public string ShowText { get; } = "show_text";
        public string ShowItem { get; } = "show_item";
        public string ShowEntity { get; } = "show_entity";
    }

    class ChatClickEvent
    {
        public ChatClickEventAction Action { get; set; }
        public string Value { get; set; }

        public ChatClickEvent(ChatClickEventAction action, string value)
        {
            Action = action;
            Value = value;
        }
    }

    class ChatHoverEvent
    {
        public ChatHoverEventAction Action { get; set; }
        public string Value { get; set; }

        public ChatHoverEvent(ChatHoverEventAction action, string value)
        {
            Action = action;
            Value = value;
        }
    }

    class ChatComponent
    {
        public short Style { get; set; }
        public char Color { get; set; }
        public string Insertion { get; set; }
        public ChatClickEvent ClickEvent { get; set; }
        public ChatHoverEvent HoverEvent { get; set; }
        public List<ChatComponent> Extra { get; set; }

        public ChatComponent()
        {
            Style = default;
            Color = default;
            Insertion = default;
            ClickEvent = null;
            HoverEvent = null;
            Extra = new List<ChatComponent>();
        }

        public virtual Dictionary<string, object> ToSerializable()
        {
            Dictionary<string, object> chatComponent = new Dictionary<string, object>();

            // Determine which flags to add
            if (Style != default)
            {
                // Calculate which styles to apply based on the style flags
                if ((Style & ChatStyle.Bold) == 1)
                    chatComponent.Add("bold", true);
                if ((Style & ChatStyle.Italic) == 1)
                    chatComponent.Add("italic", true);
                if ((Style & ChatStyle.Underlined) == 1)
                    chatComponent.Add("underlined", true);
                if ((Style & ChatStyle.Strikethrough) == 1)
                    chatComponent.Add("strikethrough", true);
                if ((Style & ChatStyle.Obfuscated) == 1)
                    chatComponent.Add("obfuscated", true);
            }

            if (Color != default)
                chatComponent.Add("color", Color);

            if (Insertion != default)
                chatComponent.Add("insertion", Insertion);

            // Events
            if (ClickEvent != null)
            {
                Dictionary<string, object> clickEvent = new Dictionary<string, object>()
                {
                    { "action", ClickEvent.Action },
                    { "value", ClickEvent.Value }
                };
                chatComponent.Add("clickEvent", clickEvent);
            }
            if (HoverEvent != null)
            {
                Dictionary<string, object> hoverEvent = new Dictionary<string, object>()
                {
                    { "action", HoverEvent.Action },
                    { "value", HoverEvent.Value }
                };
                chatComponent.Add("clickEvent", hoverEvent);
            }

            // Extend the component with more components
            if (Extra.Count > 0)
            {
                List<object> extras = new List<object>();

                // Prepare all sub-components
                foreach (ChatComponent extra in Extra)
                {
                    extras.Add(extra.ToSerializable());
                }

                chatComponent.Add("extra", extras);
            }

            return chatComponent;
        }
    }
}
