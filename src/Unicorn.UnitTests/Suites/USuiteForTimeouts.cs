﻿using System.Threading;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite for timeouts")]
    [Tag("timeouts")]
    public class USuiteForTimeouts : TestSuite
    {
        [BeforeSuite]
        public void BeforeSuite()
        {
            // Method intentionally left empty.
        }

        [BeforeTest]
        public void BeforeTest()
        {
            // Method intentionally left empty.
        }

        [Test]
        public void Test2()
        {
            Logger.Instance.Log(LogLevel.Info, "Test2 started");
            Thread.Sleep(2500);
            Logger.Instance.Log(LogLevel.Info, "Test2 finished");
        }

        [Test]
        [Disabled("")]
        public void TestToSkip()
        {
            // Method intentionally left empty.
        }

        [Test]
        public void Test1()
        {
            Logger.Instance.Log(LogLevel.Info, "Test1 started");
            Thread.Sleep(50);
            Logger.Instance.Log(LogLevel.Info, "Test1 started");
        }

        [AfterTest]
        public void AfterTest()
        {
            // Method intentionally left empty.
        }

        [AfterSuite]
        public void AfterSuite() =>
            Thread.Sleep(500);
    }
}
