namespace Bamboo.Protocol.States.Login
{
    class DisconnectPacket : ClientboundPacket
    {
        public override int PacketID { get => 0x00; }

        public DisconnectPacket(Client client) : base(client) { }

        public override void Write(IWritable buffer)
        {
            buffer.Writer.WriteVarChar("{\"text\":\"Not implemented. Playing the game will be added in future versions of Bamboo.\",\"bold\":\"true\"}");
        }
    }
}
