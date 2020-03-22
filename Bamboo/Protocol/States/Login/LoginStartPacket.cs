using Bamboo.Game;
using Bamboo.Server;
using System;

namespace Bamboo.Protocol.States.Login
{
    class LoginStartPacket : ServerboundPacket
    {
        public override int PacketID { get => 0x00; }

        public LoginStartPacket(Client client) : base(client) { }

        public override void Parse(IReadable buffer)
        {
            DataReader reader = new DataReader(buffer);

            string username = reader.ReadVarChar();

            // TODO BambooHelpers.HttpClient.GetAsync($"https://api.mojang.com/users/profiles/minecraft/{username}");
            Guid uuid = Guid.NewGuid();
            Client.Player = new Player(username, uuid);

            if (Settings.Configuration["online"] == "true")
            {
                Client.ClientboundPackets.Add(new EncryptionRequestPacket(Client));
            }
            else
            {
                Client.ClientboundPackets.Add(new SetCompressionPacket(Client));
                Client.ClientboundPackets.Add(new DisconnectPacket(Client));
                Client.ClientboundPackets.Add(new LoginSuccessPacket(Client));
            }
        }
    }
}
