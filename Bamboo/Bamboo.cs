using Microsoft.Extensions.Configuration;
using Bamboo.Server;

namespace Bamboo
{
    class Program
    {
        private static BambooServer Server;

        public Program()
        {
            // Load server configuration
            BambooSettings.LoadConfiguration();

            // Start the server
            Server = new BambooServer();
            Server.Start();

            // TODO Start the API
        }

        static void Main(string[] args)
        {
            new Program();
        }
    }
}
