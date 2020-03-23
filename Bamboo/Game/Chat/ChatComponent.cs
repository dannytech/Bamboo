using System;
using System.Collections.Generic;

namespace Bamboo.Game.Chat
{
    class ChatComponent
    {
        public int Style { get; set; }
        public string Color { get; set; }
        public string Insertion { get; set; }
        public ChatComponentClickEvent ClickEvent { get; set; }
        public ChatComponentHoverEvent HoverEvent { get; set; }
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
                if ((Style & ChatComponentStyle.Bold) == ChatComponentStyle.Bold)
                    chatComponent.Add("bold", true);
                if ((Style & ChatComponentStyle.Italic) == ChatComponentStyle.Italic)
                    chatComponent.Add("italic", true);
                if ((Style & ChatComponentStyle.Underlined) == ChatComponentStyle.Underlined)
                    chatComponent.Add("underlined", true);
                if ((Style & ChatComponentStyle.Strikethrough) == ChatComponentStyle.Strikethrough)
                    chatComponent.Add("strikethrough", true);
                if ((Style & ChatComponentStyle.Obfuscated) == ChatComponentStyle.Obfuscated)
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
