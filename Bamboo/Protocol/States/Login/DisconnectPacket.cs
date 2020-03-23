using Bamboo.Game.Chat;

namespace Bamboo.Protocol.States.Login
{
    class DisconnectPacket : ClientboundPacket
    {
        public override int PacketID { get => 0x00; }
        private readonly ChatComponent _Reason;

        public DisconnectPacket(Client client, ChatComponent reason) : base(client) {
            _Reason = reason;
        }

        public override void Write(IWritable buffer)
        {
            buffer.Writer.WriteJSON(_Reason.ToSerializable());
        }
    }
}
