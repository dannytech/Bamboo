namespace Bamboo.Protocol
{
    class BambooPacketFactory
    {
        private readonly BambooClient Client;

        public BambooPacketFactory(BambooClient client)
        {
            Client = client;
        }

        public void Parse(IReadable buffer)
        {
            BambooReader reader = new BambooReader(buffer);

            // We can assume uncompressed for the time being
            int packetId = reader.ReadVarInt();

            // Route the packet by its ID
            ServerboundPacket packet = null;
            switch (Client.ClientState)
            {
                case BambooClientState.Handshaking:
                    switch(packetId)
                    {
                        case 0x00:
                            packet = new HandshakePacket(Client);
                            break;
                    }
                    break;
                case BambooClientState.Status:
                    switch (packetId)
                    {
                        case 0x00:
                            packet = new RequestPacket(Client);
                            break;
                        case 0x01:
                            packet = new PingPacket(Client);
                            break;
                    }
                    break;
            }

            if (packet != null) packet.Parse(buffer); // Parse the packet and queue clientbound packets
        }
    }

    abstract class ClientboundPacket
    {
        public abstract int PacketID { get; }
        protected BambooClient Client;

        public ClientboundPacket(BambooClient client)
        {
            Client = client;
        }

        public abstract void Write(IWritable buffer);
    }

    abstract class ServerboundPacket
    {
        public abstract int PacketID { get; }
        protected BambooClient Client;

        protected ServerboundPacket(BambooClient client)
        {
            Client = client;
        }

        public abstract void Parse(IReadable buffer);
    }
}