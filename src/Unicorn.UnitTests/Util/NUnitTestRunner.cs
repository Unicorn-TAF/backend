using NUnit.Framework;
using System.IO;
using System.Reflection;

#pragma warning disable S2187 // TestCases should contain tests
namespace Unicorn.UnitTests.Util
{
    [TestFixture]
    public class NUnitTestRunner
    {
        protected static string DllFolder { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /* 
         * For some configurations the file is created in the root of solution instead of dll with build
         * so need to explicitly point to the folder
        */
        protected static string ConfigName { get; } = Path.Combine(DllFolder, "config.conf");

    }
}
#pragma warning restore S2187 // TestCases should contain tests
