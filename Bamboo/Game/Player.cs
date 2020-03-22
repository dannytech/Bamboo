using Bamboo.Server;
using System;

namespace Bamboo.Game
{
    class Player
    {
        public string Username;
        public Guid UUID;

        public Player(string username, Guid uuid)
        {
            Username = username;
            UUID = uuid;
        }
    }
}
