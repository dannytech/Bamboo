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
            Server server = Server.Instance;

            // Generate an object to serialize into the response JSON
            Dictionary<string, object> root = new Dictionary<string, object>();

            // Protocol metadata
            Dictionary<string, object> version = new Dictionary<string, object>
            {
                { "name", $"{Settings.ServerName} {Settings.ServerVersion}" },
                { "protocol", Settings.ProtocolVersion }
            };
            root.Add("version", version);

            // Player/capacity info
            Dictionary<string, object> capacity = new Dictionary<string, object>
            {
                { "max", 5 },
                { "online", server.Clients.Players.Length }
            };

            List<Dictionary<string, string>> playerSample = new List<Dictionary<string, string>>();
            foreach (Player player in server.Clients.Players)
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
            List<ChatTextComponent> components = ChatComponentFactory.FromString(Settings.Configuration["motd"]);
            List<Dictionary<string, object>> motd = new List<Dictionary<string, object>>();
            foreach(ChatTextComponent component in components)
            {
                motd.Add(component.ToSerializable());
            }
            root.Add("description", motd);

            // Favicon
            if (Settings.Icon != null)
            {
                root.Add("favicon", Settings.Icon);
            }

            buffer.Writer.WriteJson(root);
        }
    }
}
