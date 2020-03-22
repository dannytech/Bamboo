using Microsoft.Extensions.Configuration;

namespace Bamboo.Server
{
    class BambooSettings
    {
        internal static int ProtocolVersion = 578;
        internal static string SemanticVersion = "1.15.2";
        internal static string ServerName = "Bamboo";
        public static IConfiguration Configuration;

        internal static void LoadConfiguration()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("bamboo.json");
            Configuration = configurationBuilder.Build();
        }
    }
}
