using Bamboo.Server;
using System;

namespace Bamboo.Game
{
    class BambooPlayer
    {
        public string Username;
        public Guid UUID;

        public BambooPlayer(string username, Guid uuid)
        {
            Username = username;
            UUID = uuid;
        }
    }
}
