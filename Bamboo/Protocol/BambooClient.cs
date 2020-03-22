using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Bamboo.Protocol
{
    public enum BambooClientState
    {
        Handshaking = 0,
        Status = 1,
        Login = 2,
        Play = 3
    }

    class BambooClient
    {
        private readonly TcpClient Client;
        private readonly BambooStream Stream;
        public readonly List<ClientboundPacket> ClientboundPackets;
        public BambooClientState ClientState;

        public BambooClient(TcpClient client)
        {
            // Set up connection helpers
            Client = client;
            Stream = new BambooStream(Client.GetStream());
            ClientboundPackets = new List<ClientboundPacket>();
            
            // Set initial client state
            ClientState = BambooClientState.Handshaking;

            new Task(ServerboundTasks).Start(); // Listen for requests
            new Task(ClientboundTasks).Start(); // Send queued responses
        }

        public void Stop()
        {
            Client.Close();
        }

        private void ClientboundTasks()
        {
            while (Client.Connected)
            {
                if (ClientboundPackets.Count > 0)
                {
                    // Build a temporary write buffer
                    BambooBuffer buffer = new BambooBuffer();
                    BambooWriter writer = new BambooWriter(buffer);

                    // Grab the first packet
                    ClientboundPacket packet = ClientboundPackets[0];

                    writer.WriteVarInt(packet.PacketID); // Write the packet ID
                    packet.Write(buffer); // Fill the temporary buffer with the response packet

                    // Write the buffer to the clientbound stream along with relevant metadata
                    byte[] bytes = buffer.Buffer.ToArray();

                    // Build a secondary buffer
                    buffer = new BambooBuffer();
                    writer = new BambooWriter(buffer);
                    
                    // Prepend the packet size
                    writer.WriteVarInt(bytes.Length);
                    writer.Write(bytes);

                    // Send the bytes to the client
                    writer = new BambooWriter(Stream);
                    writer.Write(buffer.Buffer.ToArray());

                    ClientboundPackets.RemoveAt(0);
                }
            }
        }

        private void ServerboundTasks()
        {
            BambooPacketFactory factory = new BambooPacketFactory(this);

            while (Client.Connected)
            {
                BambooReader reader = new BambooReader(Stream);

                // Read the packet length
                int packetLength = reader.ReadVarInt();

                // A packet length of zero means the connection has ended on the client (a FIN packet has been sent)
                if (packetLength == 0)
                {
                    break;
                }

                // Read an entire packet
                byte[] bytes = reader.Read(packetLength);

                // Build a buffer and reader
                BambooBuffer readBuffer = new BambooBuffer(bytes);

                // Parse the packet and allow it to add clientbound packets
                factory.Parse(readBuffer);
            }

            Client.Close();
        }
    }
}
