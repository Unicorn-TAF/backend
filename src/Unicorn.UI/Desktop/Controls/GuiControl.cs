﻿using System;
using System.Windows;
using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.UserInput;
using Unicorn.UI.Desktop.Driver;

namespace Unicorn.UI.Desktop.Controls
{
    /// <summary>
    /// Represents basic abstract windows control. Contains number of main properties and action under the control.
    /// </summary>
    public abstract class GuiControl : GuiSearchContext, IControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuiControl"/> class.
        /// </summary>
        protected GuiControl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiControl"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        protected GuiControl(AutomationElement instance)
        {
            this.Instance = instance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether need to cache the control.
        /// Cached control is not searched for on each next call. Not cached control is searched each time (as PageObject control).
        /// </summary>
        public bool Cached { get; set; } = true;

        /// <summary>
        /// Gets or sets locator to find control by.
        /// </summary>
        public ByLocator Locator { get; set; }

        /// <summary>
        /// Gets or sets control name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets UI automation element class name.
        /// </summary>
        public virtual string ClassName => null;

        /// <summary>
        /// Gets UI Automation element type.
        /// </summary>
        public abstract ControlType UiaType { get; }

        /// <summary>
        /// Gets or sets control wrapped instance as <see cref="AutomationElement"/> which is also current search context.
        /// </summary>
        public virtual AutomationElement Instance
        {
            get
            {
                return this.SearchContext;
            }

            set
            {
                this.SearchContext = value;
            }
        }

        /// <summary>
        /// Gets control text.
        /// </summary>
        public virtual string Text =>
            this.Instance.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;

        /// <summary>
        /// Gets a value indicating whether control is enabled in UI.
        /// </summary>
        public bool Enabled =>
            (bool)this.Instance.GetCurrentPropertyValue(AutomationElement.IsEnabledProperty);

        /// <summary>
        /// Gets a value indicating whether control is visible (not is Off-screen)
        /// </summary>
        public bool Visible
        {
            get
            {
                try
                {
                    return !this.Instance.Current.IsOffscreen;
                }
                catch (ElementNotAvailableException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets control location as <see cref="Point"/>
        /// </summary>
        public System.Drawing.Point Location =>
            new System.Drawing.Point(this.BoundingRectangle.Location.X, this.BoundingRectangle.Location.Y);

        /// <summary>
        /// Gets control bounding rectangle as <see cref="System.Drawing.Rectangle"/>
        /// </summary>
        public System.Drawing.Rectangle BoundingRectangle =>
            (System.Drawing.Rectangle)this.Instance.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);

        /// <summary>
        /// Gets or sets control search context. 
        /// If control is not cached current context is searched from parent context by this control locator.
        /// </summary>
        public override AutomationElement SearchContext
        {
            get
            {
                if (!this.Cached)
                {
                    base.SearchContext = GetNativeControlFromParentContext(this.Locator, this.GetType());
                }

                return base.SearchContext;
            }

            set
            {
                base.SearchContext = value;
            }
        }

        /// <summary>
        /// Gets control attribute value as <see cref="string"/>
        /// </summary>
        /// <param name="attribute">attribute name</param>
        /// <returns>control attribute value as string</returns>
        public string GetAttribute(string attribute)
        {
            switch (attribute.ToLower())
            {
                case "class":
                    return (string)this.Instance.GetCurrentPropertyValue(AutomationElement.ClassNameProperty);
                case "text":
                    return (string)this.Instance.GetCurrentPropertyValue(AutomationElement.NameProperty);
                case "enabled":
                    return this.Enabled.ToString();
                case "visible":
                    return this.Visible.ToString();
                default:
                    throw new ArgumentException($"No such property as {attribute}");
            }
        }

        /// <summary>
        /// Performs click on the control.<para/>
        /// - if control has <see cref="InvokePattern"/> then control is invoked<para/>
        /// - else if control has <see cref="TogglePattern"/> then control is toggled<para/>
        /// - else mouse click is performed.
        /// </summary>
        public void Click()
        {
            Logger.Instance.Log(LogLevel.Debug, "Click " + this);
            object pattern = null;

            try
            {
                if (this.Instance.TryGetCurrentPattern(InvokePattern.Pattern, out pattern))
                {
                    ((InvokePattern)pattern).Invoke();
                }
                else
                {
                    ((TogglePattern)this.Instance.GetCurrentPattern(TogglePattern.Pattern)).Toggle();
                }
            }
            catch
            {
                MouseClick();
            }
        }

        /// <summary>
        /// Performs mouse click on the control on its clickable coordinates. If no such, click on center of control.
        /// </summary>
        public void MouseClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "Mouse click " + this);
            var point = GetClickablePoint();
            Mouse.Instance.Click(point);
        }

        /// <summary>
        /// Performs right click by mouse on the control on its clickable coordinates. If no such, click on center of control..
        /// </summary>
        public void RightClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "Right click " + this);
            var point = GetClickablePoint();
            Mouse.Instance.RightClick(point);
        }

        /// <summary>
        /// Gets parent control as <see cref="AutomationElement"/>.
        /// </summary>
        /// <returns>parent of the control as <see cref="AutomationElement"/></returns>
        public AutomationElement GetParent() =>
            TreeWalker.ControlViewWalker.GetParent(this.Instance);

        /// <summary>
        /// Gets string description of the control.
        /// </summary>
        /// <returns>control description as string</returns>
        public override string ToString() => 
            string.IsNullOrEmpty(this.Name) ? 
            $"{this.GetType().Name} [{this.Locator?.ToString()}]" : 
            this.Name;

        #region "Helpers"

        /// <summary>
        /// Get pattern of the specified type from the control.
        /// </summary>
        /// <typeparam name="T">pattern type</typeparam>
        /// <returns>pattern instance</returns>
        protected T GetPattern<T>() where T : BasePattern
        {
            var pattern = (AutomationPattern)typeof(T).GetField("Pattern").GetValue(null);
            object patternObject;
            this.Instance.TryGetCurrentPattern(pattern, out patternObject);
            return patternObject as T;
        }

        private Point GetClickablePoint()
        {
            Point point;

            if (!this.Instance.TryGetClickablePoint(out point))
            {
                Logger.Instance.Log(LogLevel.Trace, "Clickable point is not found, clicking on center of control...");

                var rect = this.BoundingRectangle;
                point = new Point(rect.Left, rect.Top);
                point.Offset(rect.Width / 2d, rect.Height / 2d);
            }

            return point;
        }

        #endregion
    }
}
