using Bamboo.Protocol.States.Handshake;
using Bamboo.Protocol.States.Status;
using Bamboo.Protocol.States.Login;

namespace Bamboo.Protocol
{
    class BambooPacketFactory
    {
        private readonly Client _Client;

        public BambooPacketFactory(Client client)
        {
            _Client = client;
        }

        public void Parse(IReadable buffer)
        {
            // We can assume uncompressed for the time being
            int packetId = buffer.Reader.ReadVarInt();

            // Route the packet by its ID
            ServerboundPacket packet = null;
            switch (_Client.ClientState)
            {
                case ClientState.Handshaking:
                    switch(packetId)
                    {
                        case 0x00:
                            packet = new HandshakePacket(_Client);
                            break;
                    }
                    break;
                case ClientState.Status:
                    switch (packetId)
                    {
                        case 0x00:
                            packet = new RequestPacket(_Client);
                            break;
                        case 0x01:
                            packet = new PingPacket(_Client);
                            break;
                    }
                    break;
                case ClientState.Login:
                    switch(packetId)
                    {
                        case 0x00:
                            packet = new LoginStartPacket(_Client);
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
        protected Client _Client;

        public ClientboundPacket(Client client)
        {
            _Client = client;
        }

        public abstract void Write(IWritable buffer);
    }

    abstract class ServerboundPacket
    {
        public abstract int PacketID { get; }
        protected Client _Client;

        protected ServerboundPacket(Client client)
        {
            _Client = client;
        }

        public abstract void Parse(IReadable buffer);
    }
}