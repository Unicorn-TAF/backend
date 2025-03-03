using System.IO;

namespace Unicorn.UnitTests
{
    internal static class DataUtils
    {
        internal static string GetDataFrom(string dataFile) =>
            File.ReadAllText(Path.Combine("TestData", dataFile));
    }
}
