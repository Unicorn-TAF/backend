﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents class container of <see cref="Test"/> and <see cref="SuiteMethod"/><para/>
    /// Contains list of events related to different Suite states (started, finished, skipped)<para/>
    /// Could have <see cref="ParameterizedAttribute"/> (the class should contain parameterized constructor with corresponding parameters)<para/>
    /// Each class with tests should be inherited from <see cref="TestSuite"/>
    /// </summary>
    public class TestSuite
    {
        private readonly Test[] tests;
        private readonly SuiteMethod[] beforeSuites;
        private readonly SuiteMethod[] beforeTests;
        private readonly SuiteMethod[] afterTests;
        private readonly SuiteMethod[] afterSuites;

        private HashSet<string> tags = null;
        private bool skipTests = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuite"/> class.<para/>
        /// On Initialize the list of Tests, BeforeTests, AfterTests, BeforeSuites and AfterSuites is retrieved from the instance.<para/>
        /// For each test is performed check for skip
        /// </summary>
        public TestSuite()
        {
            this.Metadata = new Dictionary<string, string>();

            foreach (var attribute in GetType().GetCustomAttributes(typeof(MetadataAttribute), true) as MetadataAttribute[])
            {
                if (!this.Metadata.ContainsKey(attribute.Key))
                {
                    this.Metadata.Add(attribute.Key, attribute.Value);
                }
            }

            var suiteAttribute = GetType().GetCustomAttribute(typeof(SuiteAttribute), true) as SuiteAttribute;

            this.Outcome = new SuiteOutcome();
            this.Outcome.Name = suiteAttribute != null ? suiteAttribute.Name : GetType().Name.Split('.').Last();
            this.Outcome.Id = Guid.NewGuid();
            this.Outcome.Result = Status.Passed;

            this.beforeSuites = GetSuiteMethodsByAttribute(typeof(BeforeSuiteAttribute), SuiteMethodType.BeforeSuite);
            this.beforeTests = GetSuiteMethodsByAttribute(typeof(BeforeTestAttribute), SuiteMethodType.BeforeTest);
            this.afterTests = GetSuiteMethodsByAttribute(typeof(AfterTestAttribute), SuiteMethodType.AfterTest);
            this.afterSuites = GetSuiteMethodsByAttribute(typeof(AfterSuiteAttribute), SuiteMethodType.AfterSuite);
            this.tests = GetTests();
        }

        /// <summary>
        /// Delegate used for suite events invocation
        /// </summary>
        /// <param name="testSuite">current <see cref="TestSuite"/> instance</param>
        public delegate void UnicornSuiteEvent(TestSuite testSuite);

        /// <summary>
        /// Event is invoked before suite execution
        /// </summary>
        public static event UnicornSuiteEvent OnSuiteStart;

        /// <summary>
        /// Event is invoked after suite execution
        /// </summary>
        public static event UnicornSuiteEvent OnSuiteFinish;

        /// <summary>
        /// Event is invoked if suite is skipped
        /// </summary>
        public static event UnicornSuiteEvent OnSuiteSkip;

        /// <summary>
        /// Gets test suite features. Suite could not have any feature
        /// </summary>
        public HashSet<string> Tags
        {
            get
            {
                if (this.tags == null)
                {
                    var attributes = GetType().GetCustomAttributes(typeof(TagAttribute), true) as TagAttribute[];
                    this.tags = new HashSet<string>(from attribute in attributes select attribute.Tag.ToUpper());
                }

                return this.tags;
            }
        }

        /// <summary>
        /// Gets TestSuite metadata dictionary, can contain only string values
        /// </summary>
        public Dictionary<string, string> Metadata { get; }

        /// <summary>
        /// Gets suite name (if parameterized data set name ignored)
        /// </summary>
        public string Name => this.Outcome.Name;

        /// <summary>
        /// Gets or sets Suite outcome, contain all information on suite run and results
        /// </summary>
        public SuiteOutcome Outcome { get; protected set; }

        internal void Execute()
        {
            var fullName = this.Outcome.Name;

            if (!string.IsNullOrEmpty(this.Outcome.DataSetName))
            {
                fullName += "[" + this.Outcome.DataSetName + "]";
            }

            Logger.Instance.Log(LogLevel.Info, $"==================== SUITE '{fullName}' ====================");

            var onSuiteStartPassed = false;

            try
            {
                OnSuiteStart?.Invoke(this);
                onSuiteStartPassed = true;
            }
            catch (Exception ex)
            {
                this.Skip("Exception occured during OnSuiteStart event invoke" + Environment.NewLine + ex);
            }

            if (onSuiteStartPassed)
            {
                this.RunSuite();
            }

            try
            {
                OnSuiteFinish?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnSuiteFinish event invoke" + Environment.NewLine + ex);
            }

            Logger.Instance.Log(LogLevel.Info, $"SUITE {this.Outcome.Result}");
        }

        private void RunSuite()
        {
            var suiteTimer = Stopwatch.StartNew();

            if (this.RunSuiteMethods(this.beforeSuites))
            {
                foreach (Test test in this.tests)
                {
                    this.ProcessTest(test);
                }
            }
            else
            {
                this.Skip(string.Empty);
            }

            this.RunSuiteMethods(this.afterSuites);

            suiteTimer.Stop();
            this.Outcome.ExecutionTime = suiteTimer.Elapsed;
        }

        /// <summary>
        /// Skip test suite and invoke onSkip event
        /// </summary>
        /// <param name="reason">skip reason message</param>
        private void Skip(string reason)
        {
            Logger.Instance.Log(LogLevel.Info, reason);

            foreach (Test test in this.tests)
            {
                test.Skip();
                Logger.Instance.Log(LogLevel.Warning, $"TEST '{test.Outcome.Title}' {test.Outcome.Result}");
                this.Outcome.TestsOutcomes.Add(test.Outcome);
            }

            this.Outcome.Result = Status.Skipped;

            try
            {
                OnSuiteSkip?.Invoke(this);
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnSuiteSkip event invoke" + Environment.NewLine + e);
            }
        }

        private void ProcessTest(Test test)
        {
            var dependsOnAttribute = test.TestMethod.GetCustomAttribute(typeof(DependsOnAttribute), true) as DependsOnAttribute;

            if (dependsOnAttribute != null)
            {
                var failedMainTest = this.tests
                    .Where(t => !t.Outcome.Result.Equals(Status.Passed) 
                    && t.TestMethod.Name.Equals(dependsOnAttribute.TestMethod));

                if (failedMainTest.Any())
                {
                    if (Config.DependentTests.Equals(TestsDependency.Skip))
                    {
                        test.Skip();
                        this.Outcome.TestsOutcomes.Add(test.Outcome);
                    }

                    if (!Config.DependentTests.Equals(TestsDependency.Run))
                    {
                        return;
                    }
                }
            }

            this.RunTest(test);

            this.Outcome.TestsOutcomes.Add(test.Outcome);
        }

        /// <summary>
        /// Run specified <see cref="Test"/>.
        /// </summary>
        /// <param name="test"><see cref="Test"/> instance</param>
        private void RunTest(Test test)
        {
            if (this.skipTests)
            {
                test.Skip();
                Logger.Instance.Log(LogLevel.Info, $"TEST '{test.Outcome.Title}' {Outcome.Result}");
                return;
            }

            if (!this.RunSuiteMethods(this.beforeTests))
            {
                test.Skip();
                Logger.Instance.Log(LogLevel.Info, $"TEST '{test.Outcome.Title}' {Outcome.Result}");
                return;
            }

            test.Execute(this);

            this.RunAftertests(test.Outcome.Result == Status.Failed);

            if (test.Outcome.Result == Status.Failed)
            {
                this.Outcome.Result = Status.Failed;
            }
        }

        #region Helpers

        /// <summary>
        /// Run SuiteMethods
        /// </summary>
        /// <param name="suiteMethods">array of suite methods to run</param>
        /// <returns>true if after suites run successfully; fail if at least one after suite failed</returns>
        private bool RunSuiteMethods(SuiteMethod[] suiteMethods)
        {
            foreach (var suiteMethod in suiteMethods)
            {
                suiteMethod.Execute(this);

                if (suiteMethod.Outcome.Result != Status.Passed)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Run SuiteMethods
        /// </summary>
        /// <param name="testWasFailed">array of suite methods to run</param>
        private void RunAftertests(bool testWasFailed)
        {
            foreach (var suiteMethod in this.afterTests)
            {
                var attribute = suiteMethod.TestMethod.GetCustomAttribute(typeof(AfterTestAttribute), true) as AfterTestAttribute;

                if (testWasFailed && !attribute.RunAlways)
                {
                    return;
                }

                suiteMethod.Execute(this);

                if (suiteMethod.Outcome.Result == Status.Failed)
                {
                    skipTests = attribute.SkipTestsOnFail && Config.ParallelBy != Parallelization.Test;
                }
            }
        }

        /// <summary>
        /// Get list of Tests from suite instance based on [Test] attribute presence. <para/>
        /// Determine if test should be skipped and update runnable tests count for the suite. <para/>
        /// </summary>
        /// <returns>array of <see cref="Test"/> instances</returns>
        private Test[] GetTests()
        {
            List<Test> testMethods = new List<Test>();

            IEnumerable<MethodInfo> suiteMethods = GetType().GetRuntimeMethods()
                .Where(m => m.GetCustomAttribute(typeof(TestAttribute), true) != null)
                .Where(m => AdapterUtilities.IsTestRunnable(m));

            foreach (MethodInfo method in suiteMethods)
            {
                if (AdapterUtilities.IsTestParameterized(method))
                {
                    var attribute = method.GetCustomAttribute(typeof(TestDataAttribute), true) as TestDataAttribute;
                    foreach (DataSet dataSet in AdapterUtilities.GetTestData(attribute.Method, this))
                    {
                        Test test = GenerateTest(method, dataSet);
                        testMethods.Add(test);
                    }
                }
                else
                {
                    Test test = GenerateTest(method, null);
                    testMethods.Add(test);
                }
            }

            return testMethods.ToArray();
        }

        /// <summary>
        /// Generate instance of <see cref="Test"/> and fill with all data
        /// </summary>
        /// <param name="method"><see cref="MethodInfo"/> instance which represents test method</param>
        /// <param name="dataSet"><see cref="DataSet"/> to populate test method parameters; null if method does not have parameters</param>
        /// <returns><see cref="Test"/> instance</returns>
        private Test GenerateTest(MethodInfo method, DataSet dataSet)
        {
            var test = dataSet == null ? new Test(method) : new Test(method, dataSet);
             
            test.MethodType = SuiteMethodType.Test;
            test.Outcome.ParentId = this.Outcome.Id;
            return test;
        }

        /// <summary>
        /// Get list of <see cref="MethodInfo"/> from suite instance based on specified attribute presence
        /// </summary>
        /// <param name="attributeType"><see cref="Type"/> of attribute</param>
        /// <param name="type">type of suite method (<see cref="SuiteMethodType"/>)</param>
        /// <returns>array of <see cref="SuiteMethod"/> with specified attribute</returns>
        private SuiteMethod[] GetSuiteMethodsByAttribute(Type attributeType, SuiteMethodType type)
        {
            var suitableMethods = new List<SuiteMethod>();
            var suiteMethods = GetType().GetRuntimeMethods();

            foreach (var method in suiteMethods)
            {
                var attribute = method.GetCustomAttribute(attributeType, true);

                if (attribute != null)
                {
                    var suiteMethod = new SuiteMethod(method);
                    suiteMethod.Outcome.ParentId = this.Outcome.Id;
                    suiteMethod.MethodType = type;
                    suitableMethods.Add(suiteMethod);
                }
            }

            return suitableMethods.ToArray();
        }

        #endregion
    }
}
