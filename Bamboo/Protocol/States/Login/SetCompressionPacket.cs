using Bamboo.Server;

namespace Bamboo.Protocol.States.Login
{
    class SetCompressionPacket : ClientboundPacket
    {
        public override int PacketID { get => 0x03; }

        public SetCompressionPacket(Client client) : base(client) { }

        public override void Write(IWritable buffer)
        {
            buffer.Writer.WriteVarInt(Settings.CompressionThreshold);

            _Client.Compression = CompressionState.Enabling;
        }
    }
}
