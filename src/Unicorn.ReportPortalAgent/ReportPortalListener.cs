﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ReportPortal.Client;
using ReportPortal.Shared;
using Unicorn.Core.Testing.Tests;
using Unicorn.ReportPortalAgent.Configuration;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        private static Dictionary<Status, ReportPortal.Client.Models.Status> statusMap = new Dictionary<Status, ReportPortal.Client.Models.Status>();

        private Dictionary<Guid, TestReporter> suitesFlow = new Dictionary<Guid, TestReporter>();
        private Dictionary<Guid, TestReporter> testFlowIds = new Dictionary<Guid, TestReporter>();
        private Dictionary<string, TestReporter> testFlowNames = new Dictionary<string, TestReporter>();

        static ReportPortalListener()
        {
            var configPath = Path.Combine(
                Path.GetDirectoryName(new Uri(typeof(Config).Assembly.CodeBase).LocalPath),
                "ReportPortal.conf");
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));

            Service reportPortalService;
            if (Config.Server.Proxy != null)
            {
                reportPortalService = new Service(Config.Server.Url, Config.Server.Project, Config.Server.Authentication.Uuid, new WebProxy(Config.Server.Proxy));
            }
            else
            {
                reportPortalService = new Service(Config.Server.Url, Config.Server.Project, Config.Server.Authentication.Uuid);
            }

            Bridge.Service = reportPortalService;

            statusMap[Status.Passed] = ReportPortal.Client.Models.Status.Passed;
            statusMap[Status.Failed] = ReportPortal.Client.Models.Status.Failed;
            statusMap[Status.Skipped] = ReportPortal.Client.Models.Status.Skipped;
        }

        public static Config Config
        {
            get;

            private set;
        }

        public string ExistingLaunchId
        {
            get;

            set;
        }

        public void ReportTestSkipped(Test test)
        {
            if (Config.IsEnabled)
            {
                StartTest(test);
                FinishTest(test);
            }
        }

        public void ReportLoggerMessage(Core.Logging.LogLevel level, string report)
        {
            if (Config.IsEnabled)
            {
                TestOutput(level, report);
            }
        }

        public void ReportAddAttachment(Test test, string name, string mime, byte[] content)
        {
            if (Config.IsEnabled)
            {
                AddAttachment(test, name, mime, content);
            }
        }

        public void ReportAddTestTags(Test test, params string[] tags)
        {
            if (Config.IsEnabled)
            {
                AddTestTags(test, tags);
            }
        }

        public void ReportAddSuiteTags(TestSuite suite, params string[] tags)
        {
            if (Config.IsEnabled)
            {
                AddSuiteTags(suite, tags);
            }
        }

        ////public void ReportMergeLaunches(string descriptionSearchString)
        ////{
        ////    if (Config.IsEnabled)
        ////    {
        ////        MergeRuns(descriptionSearchString);
        ////    }
        ////}

        ////public string ReportGetLaunchId(string descriptionSearchString)
        ////{
        ////    if (Config.IsEnabled)
        ////    {
        ////        return GetLaunchId(descriptionSearchString);
        ////    }
        ////    else
        ////    {
        ////        return null;
        ////    }
        ////}
    }
}
