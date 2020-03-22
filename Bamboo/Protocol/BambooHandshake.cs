namespace Bamboo.Protocol.Handshake
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

            string jsonString = "{\"version\":{\"name\":\"1.15.2\",\"protocol\":578},\"players\":{\"max\":0,\"online\":5,\"sample\":[{\"name\":\"thinkofdeath\",\"id\":\"4566e69f-c907-48ee-8d71-d7ba5aa00d20\"}]},\"description\":{\"text\":\"Hello world\"},\"favicon\":\"data:image/png;base64,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\"}";
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