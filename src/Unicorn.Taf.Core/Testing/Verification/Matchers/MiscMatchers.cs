﻿using Unicorn.Taf.Core.Testing.Verification.Matchers.MiscMatchers;

namespace Unicorn.Taf.Core.Testing.Verification
{
    public static class MiscMatchers
    {
        public static IsEvenMatcher IsEven()
        {
            return new IsEvenMatcher();
        }

        public static StringContainsMatcher StringContains(string objectToCompare)
        {
            return new StringContainsMatcher(objectToCompare);
        }
    }
}