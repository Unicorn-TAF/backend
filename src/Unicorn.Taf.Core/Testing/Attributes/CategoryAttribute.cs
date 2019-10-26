﻿using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to assign a test to some category.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class CategoryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAttribute"/> class with specified category.
        /// </summary>
        /// <param name="category">category name</param>
        public CategoryAttribute(string category)
        {
            this.Category = category;
        }

        /// <summary>
        /// Gets or sets test category.
        /// </summary>
        public string Category { get; protected set; }
    }
}
