using System;
using System.Collections.Generic;
using System.Text;

namespace Bamboo.Game.Chat
{
    class ChatScoreComponent : ChatComponent
    {
        public string Name { get; set; }
        public string Objective { get; set; }

        public ChatScoreComponent(string name, string objective) : base()
        {
            Name = name;
            Objective = objective;
        }

        public override Dictionary<string, object> ToSerializable()
        {
            Dictionary<string, object> root = base.ToSerializable();
            root.Add("name", Name);
            root.Add("objective", Objective);
            root.Add("value", 0); // TODO Implement scoreboards

            return root;
        }
    }
}
