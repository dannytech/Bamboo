namespace Bamboo.Protocol
{
    class HandshakePacket : ServerboundPacket
    {
        public override int PacketID { get => 0x00; }

        public HandshakePacket(BambooClient client) : base(client) { }

        public override void Parse(IReadable buffer)
        {
            BambooReader reader = new BambooReader(buffer);

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

    class RequestPacket : ServerboundPacket
    {
        public override int PacketID { get => 0x00; }

        public RequestPacket(BambooClient client) : base(client) { }

        public override void Parse(IReadable buffer)
        {
            Client.ClientboundPackets.Add(new ResponsePacket(Client));
        }
    }

    class ResponsePacket : ClientboundPacket
    {
        public override int PacketID { get => 0x00; }

        public ResponsePacket(BambooClient client) : base(client) { }

        public override void Write(IWritable buffer)
        {
            BambooWriter writer = new BambooWriter(buffer);

            string jsonString = "{\"version\":{\"name\":\"1.15.2\",\"protocol\":578},\"players\":{\"max\":100,\"online\":5,\"sample\":[{\"name\":\"thinkofdeath\",\"id\":\"4566e69f-c907-48ee-8d71-d7ba5aa00d20\"}]},\"description\":{\"text\":\"Hello world\"}}";
            writer.WriteVarChar(jsonString);
        }
    }

    class PingPacket : ServerboundPacket
    {
        public override int PacketID { get => 0x01; }

        public PingPacket(BambooClient client) : base(client) { }

        public override void Parse(IReadable buffer)
        {
            BambooReader reader = new BambooReader(buffer);

            // The payload for verification
            long payload = reader.ReadInt64();

            Client.ClientboundPackets.Add(new PongPacket(Client, payload));
        }
    }

    class PongPacket : ClientboundPacket
    {
        public override int PacketID { get => 0x01; }
        private long Payload;

        public PongPacket(BambooClient client, long payload) : base(client)
        {
            Payload = payload;
        }

        public override void Write(IWritable buffer)
        {
            BambooWriter writer = new BambooWriter(buffer);

            writer.WriteInt64(Payload); // Payload
        }
    }
}