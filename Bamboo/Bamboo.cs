using Microsoft.Extensions.Configuration;
using Bamboo.Server;

namespace Bamboo
{
    class Program
    {
        private static BambooServer Server;
        public IConfiguration Configuration;

        public Program()
        {
            // Load server configuration
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("./bamboo.json");
            Configuration = configurationBuilder.Build();

            // Start the server
            Server = new BambooServer(Configuration["server:ip"], ushort.Parse(Configuration["server:port"]));
            Server.Start();

            // TODO Start the API
        }

        static void Main(string[] args)
        {
            new Program();
        }
    }
}
