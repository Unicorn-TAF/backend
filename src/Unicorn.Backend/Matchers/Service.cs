using System;
using Unicorn.Backend.Matchers.RestMatchers;

namespace Unicorn.Backend.Matchers
{
    /// <summary>
    /// Entry point for Web Service matchers.
    /// </summary>
    [Obsolete("Use Unicorn.Backend.Matchers.Response instead for both SOAP and REST")]
    public static class Service
    {
        /// <summary>
        /// Gets entry point for REST service matchers.
        /// </summary>
        public static RestServiceMatchers Rest => new RestServiceMatchers();
    }
}

