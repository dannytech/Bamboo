using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Bamboo
{
    class Settings
    {
        // Bamboo constants
        internal static readonly int ProtocolVersion = 578;
        internal static readonly string ServerVersion = "1.15.2";
        internal static readonly string ServerName = "Bamboo";
        internal static readonly int VerificationTokenLength = 8;
        internal static readonly int CompressionThreshold = 64;

        // Instance settings
        public static IConfiguration Configuration;
        internal static string Icon = null;
        public static char ChatFormattingPrefix = '&';

        internal static void LoadConfiguration()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("bamboo.json");
            Configuration = configurationBuilder.Build();

            if (Configuration["icon"] != null)
            {
                byte[] bytes = File.ReadAllBytes(Configuration["icon"]);
                string base64 = Convert.ToBase64String(bytes);
                Icon = "data:image/png;base64," + base64;
            }
        }
    }
}
