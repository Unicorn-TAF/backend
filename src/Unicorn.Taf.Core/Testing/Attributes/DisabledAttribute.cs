﻿using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark a test as disabled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DisabledAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisabledAttribute"/> class with specified reason.
        /// </summary>
        /// <param name="reason">disabling reason</param>
        public DisabledAttribute(string reason)
        {
            this.Reason = reason;
        }

        /// <summary>
        /// Gets or sets disabling reason.
        /// </summary>
        public string Reason { get; protected set; }
    }
}
