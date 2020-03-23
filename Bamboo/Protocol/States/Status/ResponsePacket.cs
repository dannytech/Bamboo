using Bamboo.Game;
using Bamboo.Game.Chat;
using System.Collections.Generic;

namespace Bamboo.Protocol.States.Status
{
    class ResponsePacket : ClientboundPacket
    {
        public override int PacketID { get => 0x00; }

        public ResponsePacket(Client client) : base(client) { }

        public override void Write(IWritable buffer)
        {
            // Generate an object to serialize into the response JSON
            Dictionary<string, object> root = new Dictionary<string, object>();

            // Protocol metadata
            Dictionary<string, object> version = new Dictionary<string, object>
            {
                { "name", Settings.ServerVersion },
                { "protocol", Settings.ProtocolVersion }
            };
            root.Add("version", version);

            // Player/capacity info
            Dictionary<string, object> capacity = new Dictionary<string, object>
            {
                { "max", 5 },
                { "online", _Client.Server.Players.Length }
            };

            List<Dictionary<string, string>> playerSample = new List<Dictionary<string, string>>();

            foreach (Player player in _Client.Server.Players)
            {
                Dictionary<string, string> playerData = new Dictionary<string, string>
                {
                    { "name", player.Username },
                    { "id", player.UUID.ToString("D") }
                };

                playerSample.Add(playerData);
            }

            capacity.Add("sample", playerSample);
            root.Add("players", capacity);

            // MOTD
            Dictionary<string, object> chatComponent = new ChatTextComponent("Hello World").ToSerializable();
            root.Add("description", chatComponent);

            // Favicon
            if (Settings.Icon != null)
            {
                root.Add("favicon", Settings.Icon);
            }

            buffer.Writer.WriteJSON(root);
        }
    }
}
