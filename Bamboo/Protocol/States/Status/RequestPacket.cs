namespace Bamboo.Protocol.States.Status
{
    class RequestPacket : ServerboundPacket
    {
        public override int PacketID { get => 0x00; }

        public RequestPacket(Client client) : base(client) { }

        public override void Parse(IReadable buffer)
        {
            _Client.ClientboundPackets.Add(new ResponsePacket(_Client));
        }
    }
}
