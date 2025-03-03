using System;
using Unicorn.Taf.Core.Verification;

namespace Unicorn.UnitTests
{
    internal static class TestHelpers
    {
        internal static void CheckNegativeScenario(Action action)
        {
            try
            {
                action();
                throw new InvalidOperationException("Assertion was passed but shouldn't.");
            }
            catch (AssertionException)
            {
                // positive scenario.
            }
        }
    }
}
