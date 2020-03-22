using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Bamboo.Protocol;

namespace Bamboo.Server
{
    class BambooServer
    {
        private readonly TcpListener Server;
        private readonly IPAddress ListenIP;
        private readonly ushort ListenPort;
        private uint CurrentID = 0;
        private readonly Dictionary<uint, BambooClient> Clients = new Dictionary<uint, BambooClient>();

        public BambooServer(string ip, ushort port)
        {
            ListenIP = IPAddress.Parse(ip);
            ListenPort = port;

            Server = new TcpListener(ListenIP, ListenPort);
        }

        public void Start()
        {
            Console.WriteLine($"Starting Bamboo server on {ListenIP}:{ListenPort}...");
            Console.WriteLine("Press CTRL+C at any time to shut down the server gracefully");

            // TODO Bootstrap server before client connections are allowed

            // Start a TCP server
            Server.Start();

            // Handle graceful shutdowns via Ctrl+C
            Console.CancelKeyPress += delegate
            {
                Stop();
            };

            // Wait for incoming connections
            Server.BeginAcceptTcpClient(ClientConnection, Server);

            // We have to wait for input in order to keep the program running
            Console.Read(); // TODO Implement console
        }

        public void Stop()
        {
            Console.WriteLine("Shutting down gracefully...");

            // Disconnect all clients
            foreach (KeyValuePair<uint, BambooClient> client in Clients)
            {
                client.Value.Stop();
            }

            // Stop listening
            Server.Stop();
        }

        private void ClientConnection(IAsyncResult asyncResult)
        {
            if (Server.Server.IsBound)
            {
                // Retrieve the client connection
                TcpClient client = Server.EndAcceptTcpClient(asyncResult);

                // Handle the connection
                Clients.Add(CurrentID++, new BambooClient(client));

                // Look for the next connection
                Server.BeginAcceptTcpClient(ClientConnection, Server);
            }
        }
    }
}
