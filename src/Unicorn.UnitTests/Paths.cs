using System.IO;
using System.Reflection;

namespace Unicorn.UnitTests
{
    public class Paths
    {
        internal const string ApiBaseUrl = "https://bitbucket.org";

        internal static string DllFolder { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    }
}
