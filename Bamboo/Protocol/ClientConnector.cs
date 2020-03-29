using Bamboo.Game;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Bamboo.Protocol
{
    class ClientConnector
    {
        private readonly TcpListener _Listener;
        private readonly List<Client> _Clients;

        public Player[] Players
        {
            get
            {
                // Filter the clients to only those in the Play state and return their corresponding player objects
                return _Clients.FindAll(client => client.ClientState == ClientState.Play)
                    .ConvertAll(client => client.Player)
                    .ToArray();
            }
        }

        public ClientConnector(TcpListener server)
        {
            _Clients = new List<Client>();
            _Listener = server;
        }

        public void Connect(IAsyncResult asyncResult)
        {
            if (_Listener.Server.IsBound)
            {
                // Retrieve the client connection
                TcpClient client = _Listener.EndAcceptTcpClient(asyncResult);

                // Handle the connection
                _Clients.Add(new Client(client));


                // Look for the next connection
                _Listener.BeginAcceptTcpClient(Connect, _Listener);
            }
        }

        public void Disconnect()
        {
            // Disconnect each client
            foreach (Client client in _Clients)
            {
                client.Stop();
            }
        }
    }
}
