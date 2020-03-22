using Bamboo.Game;
using Bamboo.Server;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Bamboo.Protocol
{
    public enum ClientState
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

    class Client
    {
        private readonly TcpClient _Client;
        private readonly DataStream _Stream;
        public readonly List<ClientboundPacket> ClientboundPackets;
        public ClientState ClientState;
        public CompressionState Compression;
        public Player Player;

        public Client(TcpClient client)
        {
            // Set up connection helpers
            _Client = client;
            _Stream = new DataStream(_Client.GetStream());
            ClientboundPackets = new List<ClientboundPacket>();
            
            // Set initial client state
            ClientState = ClientState.Handshaking;
            Compression = CompressionState.Disabled;

            new Task(ServerboundTasks).Start(); // Listen for requests
            new Task(ClientboundTasks).Start(); // Send queued responses
        }

        public void Stop()
        {
            _Client.Close();
        }

        private void ClientboundTasks()
        {
            while (_Client.Connected)
            {
                if (ClientboundPackets.Count > 0)
                {
                    // Build a temporary write buffer
                    DataBuffer buffer = new DataBuffer();

                    // Grab the first packet
                    ClientboundPacket packet = ClientboundPackets[0];

                    buffer.Writer.WriteVarInt(packet.PacketID); // Write the packet ID
                    packet.Write(buffer); // Fill the temporary buffer with the response packet

                    // Write the buffer to the clientbound stream along with relevant metadata
                    byte[] bytes = buffer.Reader.ReadAll();

                    // Compress the packet
                    if (Compression == CompressionState.Enabled)
                    {
                        int uncompressedSize = bytes.Length;

                        if (uncompressedSize > Settings.CompressionThreshold)
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

                        buffer = new DataBuffer();

                        buffer.Writer.WriteVarInt(uncompressedSize); // Write the size of the uncompressed data
                        buffer.Writer.Write(bytes);

                        bytes = buffer.Reader.ReadAll();
                    }

                    // Build a buffer for the packet size and the payload
                    buffer = new DataBuffer();

                    // Prepend the packet size
                    buffer.Writer.WriteVarInt(bytes.Length);
                    buffer.Writer.Write(bytes);

                    // Send the bytes to the client
                    bytes = buffer.Reader.ReadAll();

                    _Stream.Writer.Write(bytes);

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

            while (_Client.Connected)
            {
                // Read the packet length
                int packetLength = _Stream.Reader.ReadVarInt();

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
                    if (_Stream.Reader.ReadVarInt(out int dataLengthSize) > 0)
                    {
                        // Read the compressed packet
                        byte[] compressedBytes = _Stream.Reader.Read(packetLength - dataLengthSize);

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
                        bytes = _Stream.Reader.Read(packetLength - dataLengthSize);
                    }
                }
                else
                {
                    bytes = _Stream.Reader.Read(packetLength);
                }

                // Build a buffer
                DataBuffer readBuffer = new DataBuffer(bytes);

                // Parse the packet and allow it to add clientbound packets
                factory.Parse(readBuffer);
            }

            _Client.Close();
        }
    }
}
