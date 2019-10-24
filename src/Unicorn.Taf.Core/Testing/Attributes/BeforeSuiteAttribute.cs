﻿using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark specified tests class methods as executable before all tests in suite.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BeforeSuiteAttribute : Attribute
    {
    }
}
