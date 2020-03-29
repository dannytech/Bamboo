using Bamboo.Game;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Bamboo.Protocol
{
    class Server
    {
        public static Server Instance;
        public readonly ClientConnector Clients;
        private readonly TcpListener _Listener;
        private readonly IPAddress _ListenIP;
        private readonly ushort _ListenPort;

        public Server()
        {
            _ListenIP = IPAddress.Parse(Settings.Configuration["server:ip"]);
            _ListenPort = ushort.Parse(Settings.Configuration["server:port"]);

            _Listener = new TcpListener(_ListenIP, _ListenPort);
            Clients = new ClientConnector(_Listener);

            // Set up a static reference to this instance
            Instance = this;
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
            _Listener.Start();

            // Handle graceful shutdowns via Ctrl+C
            Console.CancelKeyPress += delegate
            {
                Stop();
            };

            // Start listening for clients
            _Listener.BeginAcceptTcpClient(Clients.Connect, _Listener);

            // We have to wait for input in order to keep the program running
            Console.Read(); // TODO Implement console
        }

        public void Stop()
        {
            Console.WriteLine("Shutting down gracefully...");

            // Disconnect all clients
            Clients.Disconnect();

            // Stop listening
            _Listener.Stop();
        }
    }
}
