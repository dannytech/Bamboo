using Bamboo.Protocol;

namespace Bamboo
{
    class Program
    {
        private static Server _Server;

        public Program()
        {
            // Load server configuration
            Settings.LoadConfiguration();

            // Initialize helpers
            Helpers.Initialize();

            // Start the server
            _Server = new Server();
            _Server.Start();

            // TODO Start the API
        }

        static void Main(string[] args)
        {
            new Program();
        }
    }
}
