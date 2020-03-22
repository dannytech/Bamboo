using Bamboo.Game;
using Bamboo.Server;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

    public enum CompressionState
    {
        Disabled,
        Enabling,
        Enabled
    }

    class BambooClient
    {
        private readonly TcpClient Client;
        private readonly BambooStream Stream;
        public readonly List<ClientboundPacket> ClientboundPackets;
        public BambooClientState ClientState;
        public CompressionState Compression;
        public BambooPlayer Player;

        public BambooClient(TcpClient client)
        {
            // Set up connection helpers
            Client = client;
            Stream = new BambooStream(Client.GetStream());
            ClientboundPackets = new List<ClientboundPacket>();
            
            // Set initial client state
            ClientState = BambooClientState.Handshaking;
            Compression = CompressionState.Disabled;

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

                    // Compress the packet
                    if (Compression == CompressionState.Enabled)
                    {
                        int uncompressedSize = bytes.Length;

                        if (uncompressedSize > BambooSettings.CompressionThreshold)
                        {
                            MemoryStream uncompressed = new MemoryStream(bytes);
                            MemoryStream compressed = new MemoryStream();
                            DeflateStream zlib = new DeflateStream(compressed, CompressionMode.Compress);

                            uncompressed.CopyTo(zlib); // Compress the bytes into the compressed stream
                            zlib.Close();

                            bytes = compressed.ToArray();
                        }
                        else
                        {
                            uncompressedSize = 0; // Compression did not meet the threshold and was ignored
                        }

                        buffer = new BambooBuffer();
                        writer = new BambooWriter(buffer);

                        writer.WriteVarInt(uncompressedSize); // Write the size of the uncompressed data
                        writer.Write(bytes);

                        bytes = buffer.Buffer.ToArray();
                    }

                    // Build a buffer for the packet size and the payload
                    buffer = new BambooBuffer();
                    writer = new BambooWriter(buffer);

                    // Prepend the packet size
                    writer.WriteVarInt(bytes.Length);
                    writer.Write(bytes);

                    // Send the bytes to the client
                    bytes = buffer.Buffer.ToArray();

                    writer = new BambooWriter(Stream);
                    writer.Write(bytes);

                    ClientboundPackets.RemoveAt(0); // Remove the packet from the queue

                    // Enable compression after sending the Set Compression packet
                    if (Compression == CompressionState.Enabling && packet is States.Login.SetCompressionPacket)
                    {
                        Compression = CompressionState.Enabled;
                    }
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

                byte[] bytes;

                // Decompress the packet, if necessary
                if (Compression == CompressionState.Enabled)
                {
                    // Check that the packet met the compression threshold
                    if (reader.ReadVarInt(out int dataLengthSize) > 0)
                    {
                        // Read the compressed packet
                        byte[] compressedBytes = reader.Read(packetLength - dataLengthSize);

                        // Decompress the packet
                        MemoryStream compressed = new MemoryStream(compressedBytes);
                        MemoryStream uncompressed = new MemoryStream();
                        DeflateStream zlib = new DeflateStream(uncompressed, CompressionMode.Decompress);

                        compressed.CopyTo(zlib);
                        zlib.Close();

                        bytes = uncompressed.ToArray();
                    }
                    else
                    {
                        // Read the uncompressed packet
                        bytes = reader.Read(packetLength - dataLengthSize);
                    }
                }
                else
                {
                    bytes = reader.Read(packetLength);
                }

                // Build a buffer
                BambooBuffer readBuffer = new BambooBuffer(bytes);

                // Parse the packet and allow it to add clientbound packets
                factory.Parse(readBuffer);
            }

            Client.Close();
        }
    }
}
