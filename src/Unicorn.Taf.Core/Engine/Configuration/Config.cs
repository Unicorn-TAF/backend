﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Unicorn.Taf.Core.Engine.Configuration
{
    public enum Parallelization
    {
        Assembly,
        Suite,
        Test
    }

    public enum TestsDependency
    {
        Skip,
        DoNotRun,
        Run
    }

    public static class Config
    {
        private static HashSet<string> tags = new HashSet<string>();
        private static HashSet<string> categories = new HashSet<string>();
        private static HashSet<string> tests = new HashSet<string>();

        public static TimeSpan TestTimeout { get; set; } = TimeSpan.FromMinutes(15);

        public static TimeSpan SuiteTimeout { get; set; } = TimeSpan.FromMinutes(60);

        public static Parallelization ParallelBy { get; set; } = Parallelization.Assembly;

        public static int Threads { get; set; } = 1;

        public static TestsDependency DependentTests { get; set; } = TestsDependency.Run;

        public static HashSet<string> RunTags => tags;

        public static HashSet<string> RunCategories => categories;

        public static HashSet<string> RunTests => tests;

        /// <summary>
        /// Set tags on which test suites needed to be run.
        /// All tags are converted in upper case. Blank tags are ignored
        /// </summary>
        /// <param name="tagsToRun">array of features</param>
        public static void SetSuiteTags(params string[] tagsToRun) =>
            tags = new HashSet<string>(
                tagsToRun
                .Select(v => v.ToUpper().Trim())
                .Where(v => !string.IsNullOrEmpty(v)));
        
        /// <summary>
        /// Set tests categories needed to be run.
        /// All categories are converted in upper case. Blank categories are ignored
        /// </summary>
        /// <param name="categoriesToRun">array of categories</param>
        public static void SetTestCategories(params string[] categoriesToRun) =>
            categories = new HashSet<string>(
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
            tests = new HashSet<string>(
                testsToRun
                .Select(v => v.Trim().Replace(".", @"\.").Replace("*", "[A-z0-9]*").Replace("~", ".*"))
                .Where(v => !string.IsNullOrEmpty(v)));

        /// <summary>
        /// Deserialize run configuration fro JSON file
        /// </summary>
        /// <param name="configPath">path to JSON config file </param>
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
            tags.Clear();
            categories.Clear();
            tests.Clear();
            TestTimeout = TimeSpan.FromMinutes(15);
            SuiteTimeout = TimeSpan.FromMinutes(60);
            ParallelBy = Parallelization.Assembly;
            Threads = 1;
            DependentTests = TestsDependency.Run;
        }

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
