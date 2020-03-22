﻿using System;
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
            writer.WriteVarChar(_Client.Player.UUID.ToString("D"));
            writer.WriteVarChar(_Client.Player.Username);

            _Client.ClientState = ClientState.Play;
        }
    }
}
