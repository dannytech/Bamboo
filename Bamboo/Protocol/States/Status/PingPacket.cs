namespace Bamboo.Protocol.States.Status
{
    class PingPacket : ServerboundPacket
    {
        public override int PacketID { get => 0x01; }

        public PingPacket(Client client) : base(client) { }

        public override void Parse(IReadable buffer)
        {
            // The payload for verification
            long payload = buffer.Reader.ReadInt64();

            _Client.ClientboundPackets.Add(new PongPacket(_Client, payload));
        }
    }
}
