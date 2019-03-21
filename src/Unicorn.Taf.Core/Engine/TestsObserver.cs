﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Testing.Tests.Attributes;

namespace Unicorn.Taf.Core.Engine
{
    public static class TestsObserver
    {
        /// <summary>
        /// Search assembly for all TestSuites located inside
        /// </summary>
        /// <param name="assembly">assembly instance to search test suites for</param>
        /// <returns>collection of Type representing TestSuites</returns>
        public static IEnumerable<Type> ObserveTestSuites(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(SuiteAttribute), true).Any());
        }

        /// <summary>
        /// Search assembly for all Tests located inside
        /// </summary>
        /// <param name="assembly">assembly instance to search tests for</param>
        /// <returns>collection of MethodInfo representing Tests</returns>
        public static IEnumerable<MethodInfo> ObserveTests(Assembly assembly)
        {
            var availableTestSuites = ObserveTestSuites(assembly);

            return availableTestSuites
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                .Where(m => m.GetCustomAttributes(typeof(TestAttribute), true).Any());
        }
    }
}
