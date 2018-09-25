﻿using System.Drawing;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls
{
    public interface IControl
    {
        bool Cached { get; set; }

        /// <summary>
        /// Gets or sets element locator
        /// </summary>
        ByLocator Locator { get; set; }

        string Name { get; set; }

        #region "Props"

        bool Visible
        {
            get;
        }

        bool Enabled
        {
            get;
        }

        string Text
        {
            get;
        }

        Point Location
        {
            get;
        }

        Rectangle BoundingRectangle
        {
            get;
        }

        #endregion

        #region "Methods"

        string GetAttribute(string attribute);

        void Click();

        void RightClick();

        #endregion
    }
}