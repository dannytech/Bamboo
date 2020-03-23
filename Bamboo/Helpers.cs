using System.Net.Http;

namespace Bamboo
{
    class Helpers
    {
        // Common utilities
        internal static HttpClient HttpClient;

        internal static void Initialize()
        {
            HttpClient = new HttpClient();
        }
    }
}
