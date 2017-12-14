﻿using System;
using ReportPortal.Client;
using ReportPortal.Client.Requests;
using ReportPortal.Shared;

namespace ReportPortal.UnicornExtension.EventArguments
{
    public class TestItemStartedEventArgs : EventArgs
    {
        public TestItemStartedEventArgs(Service service, StartTestItemRequest request)
        {
            this.Service = service;
            this.TestItem = request;
        }

        public TestItemStartedEventArgs(Service service, StartTestItemRequest request, TestReporter testReporter) : this(service, request)
        {
            TestReporter = testReporter;
        }

        public Service Service { get; }

        public StartTestItemRequest TestItem { get; }

        public TestReporter TestReporter { get; set; }

        public bool Canceled { get; set; }
    }
}
