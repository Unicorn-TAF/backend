﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Unicorn.Taf.Core.Engine.Configuration
{
    /// <summary>
    /// Describes options available to control tests parallelization.
    /// </summary>
    public enum Parallelization
    {
        /// <summary>
        /// Parallel by assembly
        /// </summary>
        Assembly,

        /// <summary>
        /// Parallel by suite
        /// </summary>
        Suite,

        /// <summary>
        /// Parallel by tests within suite
        /// </summary>
        Test
    }

    /// <summary>
    /// Describes options available to control tests dependency behavior.
    /// </summary>
    public enum TestsDependency
    {
        /// <summary>
        /// Skip dependent tests if main test s failed
        /// </summary>
        Skip,
        
        /// <summary>
        /// Do not execute tests if main test s failed
        /// </summary>
        DoNotRun,
        
        /// <summary>
        /// Run tests anyway if main test s failed
        /// </summary>
        Run
    }

    /// <summary>
    /// Configures unicorn tests run parameters
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Gets or sets value indicating timeout to fail test if it reached the timeout (default - 15 minutes).
        /// </summary>
        public static TimeSpan TestTimeout { get; set; } = TimeSpan.FromMinutes(15);

        /// <summary>
        /// Gets or sets value indicating timeout to fail suite if it reached the timeout (default - 60 minutes).
        /// </summary>
        public static TimeSpan SuiteTimeout { get; set; } = TimeSpan.FromMinutes(60);

        /// <summary>
        /// Gets or sets value indicating method of parallelization of tests (default - Parallel by tests assembly).
        /// </summary>
        public static Parallelization ParallelBy { get; set; } = Parallelization.Assembly;

        /// <summary>
        /// Gets or sets value indicating number of threads to parallel on (default - 1).
        /// </summary>
        public static int Threads { get; set; } = 1;

        /// <summary>
        /// Gets or sets value indicating behavior of dependent tests if main test is failed (default - run dependent tests).
        /// </summary>
        public static TestsDependency DependentTests { get; set; } = TestsDependency.Run;

        /// <summary>
        /// Gets list of suite tags to be run (default - empty list [all suites]).
        /// </summary>
        public static HashSet<string> RunTags { get; private set; } = new HashSet<string>();

        /// <summary>
        /// Gets list of test categories to be run (default - empty list [all categories]).
        /// </summary>
        public static HashSet<string> RunCategories { get; private set; } = new HashSet<string>();

        /// <summary>
        /// Gets list of test masks to search for tests to be run (default - empty list [all tests]).
        /// </summary>
        public static HashSet<string> RunTests { get; private set; } = new HashSet<string>();

        /// <summary>
        /// Set tags on which test suites needed to be run.
        /// All tags are converted in upper case. Blank tags are ignored
        /// </summary>
        /// <param name="tagsToRun">array of features</param>
        public static void SetSuiteTags(params string[] tagsToRun) =>
            RunTags = new HashSet<string>(
                tagsToRun
                .Select(v => v.ToUpper().Trim())
                .Where(v => !string.IsNullOrEmpty(v)));
        
        /// <summary>
        /// Set tests categories needed to be run.
        /// All categories are converted in upper case. Blank categories are ignored
        /// </summary>
        /// <param name="categoriesToRun">array of categories</param>
        public static void SetTestCategories(params string[] categoriesToRun) =>
            RunCategories = new HashSet<string>(
                categoriesToRun
                .Select(v => v.ToUpper().Trim())
                .Where(v => !string.IsNullOrEmpty(v)));

        /// <summary>
        /// Set masks which filter tests to run.
        /// ~ skips any number of symbols across whole string
        /// * skips any number of symbols between dots
        /// </summary>
        /// <param name="testsToRun">tests masks</param>
        public static void SetTestsMasks(params string[] testsToRun) =>
            RunTests = new HashSet<string>(
                testsToRun
                .Select(v => v.Trim().Replace(".", @"\.").Replace("*", "[A-z0-9]*").Replace("~", ".*"))
                .Where(v => !string.IsNullOrEmpty(v)));

        /// <summary>
        /// Deserialize run configuration fro JSON file
        /// </summary>
        /// <param name="configPath">path to JSON config file</param>
        public static void FillFromFile(string configPath)
        {
            if (string.IsNullOrEmpty(configPath))
            {
                configPath = Path.GetDirectoryName(new Uri(typeof(Config).Assembly.CodeBase).LocalPath) + "/unicorn.conf";
            }

            var conf = JsonConvert.DeserializeObject<JsonConfig>(File.ReadAllText(configPath));

            TestTimeout = TimeSpan.FromMinutes(conf.JsonTestTimeout);
            SuiteTimeout = TimeSpan.FromMinutes(conf.JsonSuiteTimeout);
            ParallelBy = GetEnumValue<Parallelization>(conf.JsonParallelBy);
            Threads = conf.JsonThreads;
            DependentTests = GetEnumValue<TestsDependency>(conf.JsonTestsDependency);
            SetSuiteTags(conf.JsonRunTags.ToArray());
            SetTestCategories(conf.JsonRunCategories.ToArray());
            SetTestsMasks(conf.JsonRunTests.ToArray());
        }

        /// <summary>
        /// Reset engine config to default state
        /// </summary>
        public static void Reset()
        {
            RunTags.Clear();
            RunCategories.Clear();
            RunTests.Clear();
            TestTimeout = TimeSpan.FromMinutes(15);
            SuiteTimeout = TimeSpan.FromMinutes(60);
            ParallelBy = Parallelization.Assembly;
            Threads = 1;
            DependentTests = TestsDependency.Run;
        }

        /// <summary>
        /// Get information about run configuration in readable format.
        /// </summary>
        /// <returns>string with info</returns>
        public static string GetInfo()
        {
            const string Delimiter = ",";

            return new StringBuilder()
                .AppendLine($"Tags to run: {string.Join(Delimiter, RunTags)}")
                .AppendLine($"Categories to run: {string.Join(Delimiter, RunCategories)}")
                .AppendLine($"Tests filter: {string.Join(Delimiter, RunTests)}")
                .AppendLine($"Parallel by '{ParallelBy}' to '{Threads}' thread(s)")
                .AppendLine($"Dependent tests: '{DependentTests}'")
                .AppendLine($"Test run timeout: {TestTimeout}")
                .AppendLine($"Suite run timeout: {SuiteTimeout}")
                .ToString();
        }

        private static T GetEnumValue<T>(string jsonValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), jsonValue, true);
            }
            catch
            {
                throw new ArgumentException(
                    $"{typeof(T)} is not defined. Available methods are: " +
                    string.Join(",", Enum.GetValues(typeof(T)).Cast<T>()));
            }
        }
    }
}
