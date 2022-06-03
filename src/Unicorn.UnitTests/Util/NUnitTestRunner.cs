﻿using NUnit.Framework;
using Unicorn.Taf.Core.Logging;

#pragma warning disable S2187 // TestCases should contain tests
namespace Unicorn.UnitTests.Util
{
    [TestFixture]
    public class NUnitTestRunner
    {
        [OneTimeSetUp]
        public static void ClassInit()
        {
            Logger.Instance = new TestContextLogger();
        }
    }
}
#pragma warning restore S2187 // TestCases should contain tests
