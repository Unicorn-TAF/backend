﻿namespace Unicorn.Taf.Core.Verification.Matchers
{
    /// <summary>
    /// Base matcher for type specific realizations.
    /// </summary>
    /// <typeparam name="T">type of object under assertion</typeparam>
    public abstract class TypeSafeMatcher<T> : AbstractMatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSafeMatcher{T}"/> class.
        /// </summary>
        protected TypeSafeMatcher() : base()
        {
        }

        /// <summary>
        /// Checks if target object matches condition corresponding to specific matcher realization.
        /// </summary>
        /// <param name="actual">object under assertion</param>
        /// <returns>true - if object matches specific condition; otherwise - false</returns>
        public abstract bool Matches(T actual);
    }
}
