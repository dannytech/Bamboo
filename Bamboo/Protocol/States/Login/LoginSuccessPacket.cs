namespace Bamboo.Protocol.States.Login
{
    class LoginSuccessPacket : ClientboundPacket
    {
        public override int PacketID { get => 0x02; }

        public LoginSuccessPacket(Client client) : base(client) { }

        public override void Write(IWritable buffer)
        {
            // Write the player metadata
            buffer.Writer.WriteVarChar(_Client.Player.UUID.ToString("D"));
            buffer.Writer.WriteVarChar(_Client.Player.Username);

            _Client.ClientState = ClientState.Play;
        }
    }
}
