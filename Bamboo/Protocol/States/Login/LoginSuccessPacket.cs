using System;
using System.Collections.Generic;
using System.Text;

namespace Bamboo.Protocol.States.Login
{
    class LoginSuccessPacket : ClientboundPacket
    {
        public override int PacketID { get => 0x02; }

        public LoginSuccessPacket(Client client) : base(client) { }

        public override void Write(IWritable buffer)
        {
            DataWriter writer = new DataWriter(buffer);

            // Write the player metadata
            writer.WriteVarChar(Client.Player.UUID.ToString("D"));
            writer.WriteVarChar(Client.Player.Username);

            Client.ClientState = BambooClientState.Play;
        }
    }
}
