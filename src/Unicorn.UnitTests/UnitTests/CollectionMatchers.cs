﻿using System.Collections.Generic;
using NUnit.Framework;
using Unicorn.Taf.Core.Verification.Matchers;
using Um = Unicorn.Taf.Core.Verification.Matchers;
using Uv = Unicorn.Taf.Core.Verification;

namespace Unicorn.UnitTests.Tests
{
    [TestFixture]
    public class CollectionMatchers
    {
        #region Data
        private readonly string[] hasItemsA = new [] 
        {
            "qwerty", "qwerty12", "qwerty123"
        };

        private readonly string[] hasItemsB = new [] 
        {
            "qwerty", "qwerty123"
        };

        private readonly string[] hasItemsC = new [] 
        {
            "qwerty3", "qwerty1234"
        };

        private readonly string[] hasItemsD = new [] 
        {
            "qwerty", "qwerty1234"
        };

        private readonly string[] expected1 = new[]
            {
                "qwerty"
            };

        private readonly string[] expected2 = new[]
            {
                "qwerty6"
            };

        private readonly string[] expected3 = new[]
            {
                "qwert12y"
            };

        private readonly int[] intArray1 = new[]
            {
                2,
                3
            };

        private readonly int[] intArray2 = new[]
            {
                2,
                4,
                6
            };

        private readonly List<int> intList1 = new List<int>
            {
                3,
                2
            };

        #endregion

        #region "HasItems"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive1() =>
            Uv.Assert.That(hasItemsA, Collection.HasItems(hasItemsB));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive2() =>
            Uv.Assert.That(hasItemsA, Collection.HasItems(expected1));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive3() =>
            Uv.Assert.That(hasItemsA, Collection.HasItems(hasItemsA));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotPositive1() =>
            Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(hasItemsC)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotPositive2() =>
            Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(expected2)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Collection.HasItems(hasItemsD));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNegative2() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Collection.HasItems(expected3));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNullNegative3() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Collection.HasItems(hasItemsB));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(hasItemsB)));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative2() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(expected1)));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative3() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(hasItemsA)));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative4() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(hasItemsD)));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNullWithNotNegative5() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Um.Is.Not(Collection.HasItems(hasItemsB)));
            });

        #endregion

        #region "HasItem"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemPositive1() =>
            Uv.Assert.That(hasItemsA, Collection.HasItem("qwerty"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotPositive1() =>
            Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItem("qwerty27")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Collection.HasItem("qwerty27"));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemNullNegative2() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Collection.HasItem("qwerty"));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItem("qwerty")));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotNullNegative2() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Um.Is.Not(Collection.HasItem("qwerty")));
            });

        #endregion

        #region "IsNullOrEmpty"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive1() =>
            Uv.Assert.That(null, Collection.IsNullOrEmpty());

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive2() =>
            Uv.Assert.That(new List<string>(), Collection.IsNullOrEmpty());

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive3() =>
            Uv.Assert.That(new string[0], Collection.IsNullOrEmpty());

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Collection.IsNullOrEmpty());
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyWithNotPositive1() =>
            Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.IsNullOrEmpty()));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyWithNotPositive2() =>
            Uv.Assert.That(intArray1, Um.Is.Not(Collection.IsNullOrEmpty()));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyWithNotNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Um.Is.Not(Collection.IsNullOrEmpty()));
            });

        #endregion

        #region "Each"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherEachPositive() =>
            Uv.Assert.That(intArray2, Collection.Each(Number.IsEven()));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherEachWithNotPositive2() =>
            Uv.Assert.That(intArray1, Um.Is.Not(Collection.Each(Number.IsEven())));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherEachNullNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Collection.Each(Um.Is.EqualTo("a")));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherEachNegative() =>
           Assert.Throws<Uv.AssertionException>(delegate
           {
               Uv.Assert.That(hasItemsA, Collection.Each(Um.Is.EqualTo("a")));
           });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherEachNullWithNotNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Um.Is.Not(Collection.Each(Um.Is.EqualTo("a"))));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherEachWithNotNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(expected1, Um.Is.Not(Collection.Each(Um.Is.EqualTo("qwerty"))));
            });

        #endregion

        #region "Any"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherAnyPositive() =>
            Uv.Assert.That(hasItemsA, Collection.Any(Um.Is.EqualTo("qwerty123")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherAnyNullWithNotPositive2() =>
            Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.Any(Um.Is.EqualTo("qwerty4"))));
        
        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherAnyNullNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Collection.Any(Um.Is.EqualTo("a")));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherAnyNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(hasItemsA, Collection.Any(Um.Is.EqualTo("")));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherAnyNullWithNotNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Um.Is.Not(Collection.Any(Um.Is.EqualTo("a"))));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherAnyWithNotNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.Any(Um.Is.EqualTo("qwerty"))));
            });

        #endregion

        #region "IsTheSameAs"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsTheSameAsPositive1() =>
            Uv.Assert.That(intArray1, Collection.IsTheSameAs(intList1));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsTheSameAsWithNotPositive1() =>
            Uv.Assert.That(intArray1, Um.Is.Not(Collection.IsTheSameAs(intArray2)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsTheSameAsNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(intArray1, Collection.IsTheSameAs(intArray2));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsTheSameAsNullNegative2() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Collection.IsTheSameAs(expected1));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsTheSameAsWithNotNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(intArray1, Um.Is.Not(Collection.IsTheSameAs(intArray1)));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsTheSameAsWithNotNullNegative2() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Um.Is.Not(Collection.IsTheSameAs(hasItemsB)));
            });

        #endregion

    }
}