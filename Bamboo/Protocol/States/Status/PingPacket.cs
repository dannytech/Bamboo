namespace Bamboo.Protocol.States.Status
{
    class PingPacket : ServerboundPacket
    {
        public override int PacketID { get => 0x01; }

        public PingPacket(Client client) : base(client) { }

        public override void Parse(IReadable buffer)
        {
            DataReader reader = new DataReader(buffer);

            // The payload for verification
            long payload = reader.ReadInt64();

            Client.ClientboundPackets.Add(new PongPacket(Client, payload));
        }
    }
}
