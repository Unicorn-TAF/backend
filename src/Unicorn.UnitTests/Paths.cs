using System.IO;
using System.Reflection;

namespace Unicorn.UnitTests
{
    public class Paths
    {
        internal const string ReastApiBaseUrl = "https://api.restful-api.dev";
        internal const string SoapBaseUrl = "https://www.dataaccess.com";
        internal const string UnicornBaseUrl = "https://unicorn-taf.github.io";

        internal static string DllFolder { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
