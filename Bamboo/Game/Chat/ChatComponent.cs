using System.Collections.Generic;
using Bamboo.Protocol;

namespace Bamboo.Game.Chat
{
    class ChatComponent : ISerializable
    {
        public int Style { get; set; }
        public string Color { get; set; }
        public string Insertion { get; set; }
        public bool Reset { get; set; } // Custom flag to trigger a styling reset
        public ChatComponentClickEvent ClickEvent { get; set; }
        public ChatComponentHoverEvent HoverEvent { get; set; }
        public List<ChatComponent> Extra { get; set; }

        public ChatComponent()
        {
            Style = ChatComponentStyle.None;
            Color = null;
            Insertion = default;
            ClickEvent = null;
            HoverEvent = null;
            Extra = new List<ChatComponent>();
        }

        public virtual Dictionary<string, object> ToSerializable()
        {
            Dictionary<string, object> chatComponent = new Dictionary<string, object>();

            // Determine which flags to add
            if (Reset)
            {
                chatComponent.Add("bold", false);
                chatComponent.Add("italic", false);
                chatComponent.Add("underlined", false);
                chatComponent.Add("strikethrough", false);
                chatComponent.Add("obfuscated", false);
            }
            else if (Style != default)
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
                chatComponent.Add("clickEvent", ClickEvent.ToSerializable());

            if (HoverEvent != null)
                chatComponent.Add("hoverEvent", HoverEvent.ToSerializable());

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
