using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using Bamboo.Protocol;

namespace Bamboo.Server
{
    class Server
    {
        private readonly TcpListener _Server;
        private readonly IPAddress _ListenIP;
        private readonly ushort _ListenPort;
        private uint _CurrentID = 0;
        private readonly Dictionary<uint, Client> _Clients = new Dictionary<uint, Client>();

        public Server()
        {
            _ListenIP = IPAddress.Parse(Settings.Configuration["server:ip"]);
            _ListenPort = ushort.Parse(Settings.Configuration["server:port"]);

            _Server = new TcpListener(_ListenIP, _ListenPort);
        }

        public void Start()
        {
            Console.WriteLine($"Starting Bamboo server on {_ListenIP}:{_ListenPort}...");
            Console.WriteLine("Press CTRL+C at any time to shut down the server gracefully");

            // TODO Bootstrap server before client connections are allowed

            // Generate a server private key, if it doesn't exist already
            RSACryptoServiceProvider serverKey = new RSACryptoServiceProvider(1024);
            RSAParameters parameters = serverKey.ExportParameters(true);

            // Start a TCP server
            _Server.Start();

            // Handle graceful shutdowns via Ctrl+C
            Console.CancelKeyPress += delegate
            {
                Stop();
            };

            // Wait for incoming connections
            _Server.BeginAcceptTcpClient(ClientConnection, _Server);

            // We have to wait for input in order to keep the program running
            Console.Read(); // TODO Implement console
        }

        public void Stop()
        {
            Console.WriteLine("Shutting down gracefully...");

            // Disconnect all clients
            foreach (KeyValuePair<uint, Client> client in _Clients)
            {
                client.Value.Stop();
            }

            // Stop listening
            _Server.Stop();
        }

        private void ClientConnection(IAsyncResult asyncResult)
        {
            if (_Server.Server.IsBound)
            {
                // Retrieve the client connection
                TcpClient client = _Server.EndAcceptTcpClient(asyncResult);

                // Handle the connection
                _Clients.Add(_CurrentID++, new Client(client));

                // Look for the next connection
                _Server.BeginAcceptTcpClient(ClientConnection, _Server);
            }
        }
    }
}
