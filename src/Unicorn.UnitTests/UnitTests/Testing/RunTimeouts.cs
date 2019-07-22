﻿using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class RunTimeouts : NUnitTestRunner
    {
        private static TestsRunner runner;

        [OneTimeSetUp]
        public static void Setup()
        {
            Config.Reset();
            Config.SetSuiteTags("timeouts");
            Config.TestTimeout = TimeSpan.FromSeconds(1);
            runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Test timeout")]
        public void TestTimeoutForTest()
        {
            var outcome = runner.Outcome.SuitesOutcomes.First(o => o.Name.Equals("Suite for timeouts 1", StringComparison.InvariantCultureIgnoreCase));

            Assert.That(outcome.FailedTests, Is.EqualTo(1));
            Assert.That(outcome.TestsOutcomes.First(o => o.Result.Equals(Status.Failed)).Exception.GetType(), Is.EqualTo(typeof(TestTimeoutException)));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check BeforeSuite timeout")]
        public void TestTimeoutForBeforeSuite()
        {
            var outcome = runner.Outcome.SuitesOutcomes.First(o => o.Name.Equals("Suite for timeouts 2", StringComparison.InvariantCultureIgnoreCase));

            Assert.That(outcome.Result, Is.EqualTo(Status.Skipped));
            Assert.That(outcome.SkippedTests, Is.EqualTo(2));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check BeforeTest timeout")]
        public void TestTimeoutForBeforeTest()
        {
            var outcome = runner.Outcome.SuitesOutcomes.First(o => o.Name.Equals("Suite for timeouts 3", StringComparison.InvariantCultureIgnoreCase));

            Assert.That(outcome.Result, Is.EqualTo(Status.Passed));
            Assert.That(outcome.SkippedTests, Is.EqualTo(1));
            Assert.That(outcome.PassedTests, Is.EqualTo(1));
        }
    }
}
