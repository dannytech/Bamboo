using System.Collections.Generic;

namespace Bamboo.Game.Chat
{
    enum TranslationKey
    {
    
    }

    class ChatTranslationComponent : ChatComponent
    {
        public TranslationKey Key { get; set; }
        public ChatComponent[] With { get; set; }

        public ChatTranslationComponent(TranslationKey key) : this(key, new ChatComponent[0]) { }
        public ChatTranslationComponent(TranslationKey key, ChatComponent[] with) : base()
        {
            Key = key;
            With = with;
        }

        public override Dictionary<string, object> ToSerializable()
        {
            Dictionary<string, object> root = base.ToSerializable();
            root.Add("translate", Key);
            
            // Parameters to interpolate into the translated string
            if (With.Length > 0)
            {
                List<object> withComponents = new List<object>();

                foreach (ChatComponent chatComponent in With)
                {
                    withComponents.Add(chatComponent.ToSerializable());
                }

                root.Add("with", withComponents);
            }

            return root;
        }
    }
}
