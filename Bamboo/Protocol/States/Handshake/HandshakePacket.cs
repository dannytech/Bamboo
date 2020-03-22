namespace Bamboo.Protocol.States.Handshake
{
    class HandshakePacket : ServerboundPacket
    {
        public override int PacketID { get => 0x00; }

        public HandshakePacket(Client client) : base(client) { }

        public override void Parse(IReadable buffer)
        {
            // First, the protocol version
            buffer.Reader.ReadVarInt();

            // Then, the hostname
            buffer.Reader.ReadVarChar();

            // Then, the port
            buffer.Reader.ReadUInt16();

            // Lastly, the type of request (status or login)
            int nextState = buffer.Reader.ReadVarInt();
            switch (nextState)
            {
                case 1:
                    _Client.ClientState = ClientState.Status;
                    break;
                case 2:
                    _Client.ClientState = ClientState.Login;
                    break;
            }
        }
    }
}