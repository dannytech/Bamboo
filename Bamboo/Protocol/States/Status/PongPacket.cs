namespace Bamboo.Protocol.States.Status
{
    class PongPacket : ClientboundPacket
    {
        public override int PacketID { get => 0x01; }
        private readonly long _Payload;

        public PongPacket(Client client, long payload) : base(client)
        {
            _Payload = payload;
        }

        public override void Write(IWritable buffer)
        {
            buffer.Writer.WriteInt64(_Payload); // Payload
        }
    }
}
