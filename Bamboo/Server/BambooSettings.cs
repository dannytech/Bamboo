using Microsoft.Extensions.Configuration;

namespace Bamboo.Server
{
    class BambooSettings
    {
        // Bamboo constants
        internal static readonly int ProtocolVersion = 578;
        internal static readonly string SemanticVersion = "1.15.2";
        internal static readonly string ServerName = "Bamboo";
        internal static readonly int VerificationTokenLength = 8;
        internal static readonly int CompressionThreshold = 64;

        // Instance settings
        public static IConfiguration Configuration;

        internal static void LoadConfiguration()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("bamboo.json");
            Configuration = configurationBuilder.Build();
        }
    }
}
