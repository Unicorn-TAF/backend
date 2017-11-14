﻿using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.UnicornExtension.EventArguments;
using ReportPortal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Unicorn.Core.Testing.Tests;
using System.IO;
using Unicorn.Core.Reporting;
using System.Text;

namespace ReportPortal.UnicornExtension
{
    public partial class ReportPortalListener
    {
        public delegate void TestStartedHandler(object sender, TestItemStartedEventArgs e);

        public static event TestStartedHandler BeforeTestStarted;
        public static event TestStartedHandler AfterTestStarted;

        Test CurrentTest = null;

        public void StartTest(Test test)
        {
            try
            {
                var id = test.Id.ToString();
                var parentId = test.ParentId.ToString();
                var name = test.Description;
                var fullname = test.FullTestName;

                CurrentTest = test;

                var startTestRequest = new StartTestItemRequest
                {
                    StartTime = DateTime.UtcNow,
                    Name = name,
                    Type = TestItemType.Step
                };

                var beforeTestEventArg = new TestItemStartedEventArgs(Bridge.Service, startTestRequest);
                try
                {
                    BeforeTestStarted?.Invoke(this, beforeTestEventArg);
                }
                catch (Exception exp)
                {
                    Console.WriteLine("Exception was thrown in 'BeforeTestStarted' subscriber." + Environment.NewLine +
                                      exp);
                }
                if (!beforeTestEventArg.Canceled)
                {
                    var testVal = _suitesFlow[parentId].StartNewTestNode(startTestRequest);

                    _testFlowIds[id] = testVal;

                    _testFlowNames[fullname] = testVal;

                    try
                    {
                        AfterTestStarted?.Invoke(this, new TestItemStartedEventArgs(Bridge.Service, startTestRequest, testVal));
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine("Exception was thrown in 'AfterTestStarted' subscriber." + Environment.NewLine +
                                          exp);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        public delegate void TestFinishedHandler(object sender, TestItemFinishedEventArgs e);

        public static event TestFinishedHandler BeforeTestFinished;
        public static event TestFinishedHandler AfterTestFinished;

        public void FinishTest(Test test)
        {
            try
            {
                var id = test.Id.ToString();
                var result = test.Outcome.Result.ToString();
                var parentId = test.ParentId.ToString();

                CurrentTest = null;

                if (_testFlowIds.ContainsKey(id))
                {
                    var updateTestRequest = new UpdateTestItemRequest();

                    // adding categories to test
                    var categories = test.Categories;

                    updateTestRequest.Tags = new List<string>();
                    updateTestRequest.Tags.Add(test.Author);
                    if (categories != null)
                    {
                        foreach (string category in categories)
                            updateTestRequest.Tags.Add(category);
                    }

                    //add tag for fail reason
                    if (test.Outcome.Result == Result.FAILED)
                    {
                        try
                        {
                            var rez = from v in test.Outcome.Bugs
                                      where
                                          (v.Trim().Equals("?")) ||
                                          (v.ToLower().Contains("closed")) ||
                                          (v.ToLower().Contains("rejected"))
                                      select v;

                            if (rez.Count() > 0)
                                updateTestRequest.Tags.Add("TO_INVESTIGATE");
                            else
                                updateTestRequest.Tags.Add("FAILED_BY_OPEN_BUG");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Unable to get tag for fail reason." + Environment.NewLine + ex);
                        }
                    }


                    // adding description to test
                    var description = test.Description;
                    if (description != null)
                        updateTestRequest.Description = description;

                    if (updateTestRequest.Description != null || updateTestRequest.Tags != null)
                        _testFlowIds[id].Update(updateTestRequest);

                    // adding failure items
                    
                    if (test.Outcome.Result == Result.FAILED)
                    {
                        var failureMessage = test.Outcome.Exception.Message;
                        var failureStacktrace = test.Outcome.Exception.StackTrace;

                        if (!string.IsNullOrEmpty(test.Outcome.Screenshot))
                        {
                            byte[] screenshotBytes = File.ReadAllBytes(Path.Combine(Screenshot.SCREENSHOTS_FOLDER, test.Outcome.Screenshot));

                            _testFlowIds[id].Log(new AddLogItemRequest
                            {
                                Level = LogLevel.Error,
                                Time = DateTime.UtcNow,
                                Text = failureMessage + Environment.NewLine + failureStacktrace,
                                Attach = new Attach(test.Outcome.Screenshot, "image/jpeg", screenshotBytes)
                            });
                        }
                        else
                        {
                            _testFlowIds[id].Log(new AddLogItemRequest
                            {
                                Level = LogLevel.Error,
                                Time = DateTime.UtcNow,
                                Text = failureMessage + Environment.NewLine + failureStacktrace,
                            });
                        }

                        _testFlowIds[id].Log(new AddLogItemRequest
                        {
                            Level = LogLevel.Error,
                            Time = DateTime.UtcNow,
                            Text = "Attachment: Log file",
                            Attach = new Attach(test.Outcome.Screenshot, "text/plain", Encoding.ASCII.GetBytes(Test.CurrentOutput.ToString()))
                        });
                    }

                    // finishing test
                    var finishTestRequest = new FinishTestItemRequest
                    {
                        EndTime = DateTime.UtcNow,
                        Status = _statusMap[result]
                    };

                    var eventArg = new TestItemFinishedEventArgs(Bridge.Service, finishTestRequest, _testFlowIds[id]);

                    try
                    {
                        BeforeTestFinished?.Invoke(this, eventArg);
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine("Exception was thrown in 'BeforeTestFinished' subscriber." +
                                          Environment.NewLine + exp);
                    }

                    _testFlowIds[id].Finish(finishTestRequest);

                    try
                    {
                        AfterTestFinished?.Invoke(this,
                            new TestItemFinishedEventArgs(Bridge.Service, finishTestRequest, _testFlowIds[id]));
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine("Exception was thrown in 'AfterTestFinished' subscriber." +
                                          Environment.NewLine + exp);
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        public void TestOutput(string info)
        {
            try
            {
                var fullTestName = CurrentTest.FullTestName;
                var message = info;

                if (_testFlowNames.ContainsKey(fullTestName))
                {
                    var serializer = new JavaScriptSerializer {MaxJsonLength = int.MaxValue};
                    AddLogItemRequest logRequest = null;
                    try
                    {
                        logRequest = serializer.Deserialize<AddLogItemRequest>(message);
                    }
                    catch (Exception)
                    {

                    }
                    
                    if (logRequest != null)
                        _testFlowNames[fullTestName].Log(logRequest);
                    else
                        _testFlowNames[fullTestName].Log(new AddLogItemRequest { Level = LogLevel.Info, Time = DateTime.UtcNow, Text = message});
                    
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}