﻿using Core.Logging;
using Core.Testing;
using Core.Testing.Attributes;
using ProjectSpecific.BO;

namespace ProjectSpecific.Steps
{
    public class SampleSteps : TestSteps
    {

        [TestStep("First Test Step")]
        public void FirstTestStep()
        {
            ReportStep();
            Logger.Info("");
        }

        [TestStep("Second Test Step '{0}'")]
        public void SecondTestStep(string value)
        {
            ReportStep(value);
            Logger.Info("");

        }

        [TestStep("Third Test Step '{0}'")]
        public int ThirdTestStep(int a)
        {
            ReportStep(a);
            Logger.Info("");
            return a;
        }

        [TestStep("Fourth Test Step '{0}'")]
        public void FourthTestStep(SampleObject a)
        {
            ReportStep(a);
            Logger.Info("");
        }
    }
}
