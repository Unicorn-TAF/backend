﻿using NUnit.Framework;
using Unicorn.Core.Logging;

namespace Unicorn.UnitTests.Util
{
    public class TestContextLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            TestContext.WriteLine($"{GetIndent(level)}{level}: {message}");
        }

        private string GetIndent(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return "|\t";
                case LogLevel.Trace:
                    return "|\t\t";
                default:
                    return string.Empty;
            }
        }
    }
}
