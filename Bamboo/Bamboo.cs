using Microsoft.Extensions.Configuration;
using Bamboo.Server;

namespace Bamboo
{
    class Program
    {
        private static Server.Server Server;

        public Program()
        {
            // Load server configuration
            Settings.LoadConfiguration();

            // Initialize helpers
            Helpers.Initialize();

            // Start the server
            Server = new Server.Server();
            Server.Start();

            // TODO Start the API
        }

        static void Main(string[] args)
        {
            new Program();
        }
    }
}
