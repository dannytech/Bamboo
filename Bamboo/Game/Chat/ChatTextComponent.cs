using System.Collections.Generic;

namespace Bamboo.Game.Chat
{
    class ChatTextComponent : ChatComponent
    {
        public string Text { get; set; }

        public ChatTextComponent(string text) : base()
        {
            Text = text;
        }

        public override Dictionary<string, object> ToSerializable()
        {
            Dictionary<string, object> root = base.ToSerializable();
            root.Add("text", Text);

            return root;
        }
    }
}
