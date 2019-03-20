﻿using System;
using AspectInjector.Broker;
using Unicorn.Taf.Core.Testing.Steps.Attributes;
using Unicorn.Taf.Core.Testing.Verification;
using Unicorn.Taf.Core.Testing.Verification.Matchers;

namespace Unicorn.Taf.Core.Testing.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class AssertionSteps
    {
        private Verify softAssertion = null;

        [TestStep("Assert that '{0}' {1}")]
        public void AssertThat<T>(T actual, TypeSafeMatcher<T> matcher) => 
            Assert.That(actual, matcher);

        [TestStep("Assert that '{0}' {1}")]
        public void AssertThat(object actual, Matcher matcher) =>
            Assert.That(actual, matcher);

        public AssertionSteps StartVerification()
        {
            softAssertion = new Verify();
            return this;
        }

        [TestStep("Verify that '{0}' {1}")]
        public AssertionSteps VerifyThat<T>(T actual, TypeSafeMatcher<T> matcher)
        {
            if (softAssertion == null)
            {
                softAssertion = new Verify();
            }

            softAssertion.VerifyThat(actual, matcher);
            return this;
        }

        [TestStep("Verify that '{0}' {1}")]
        public AssertionSteps VerifyThat(object actual, Matcher matcher)
        {
            if (softAssertion == null)
            {
                softAssertion = new Verify();
            }

            softAssertion.VerifyThat(actual, matcher);
            return this;
        }

        [TestStep("Assert verifications chain")]
        public void AssertVerificationsChain()
        {
            if (softAssertion == null)
            {
                throw new InvalidOperationException("There were no any verifications made. Please check scenario.");
            }

            try
            {
                softAssertion.AssertAll();
            }
            finally
            {
                softAssertion = null;
            }
        }
    }
}