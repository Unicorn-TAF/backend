﻿using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to assign a bug to test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class BugAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BugAttribute"/> class with specified bug reference.
        /// </summary>
        /// <param name="bug">bug id or reference</param>
        public BugAttribute(string bug)
        {
            this.Bug = bug;
        }

        /// <summary>
        /// Gets or sets test bug.
        /// </summary>
        public string Bug { get; protected set; }
    }
}
