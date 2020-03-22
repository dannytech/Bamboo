namespace Bamboo.Protocol.States.Handshake
{
    class HandshakePacket : ServerboundPacket
    {
        public override int PacketID { get => 0x00; }

        public HandshakePacket(Client client) : base(client) { }

        public override void Parse(IReadable buffer)
        {
            DataReader reader = new DataReader(buffer);

            // First, the protocol version
            reader.ReadVarInt();

            // Then, the hostname
            reader.ReadVarChar();

            // Then, the port
            reader.ReadUInt16();

            // Lastly, the type of request (status or login)
            BambooClientState nextState = (BambooClientState)reader.ReadVarInt();

            Client.ClientState = nextState;
        }
    }
}