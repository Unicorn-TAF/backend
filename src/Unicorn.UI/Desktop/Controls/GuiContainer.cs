﻿using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.Controls.Typified;

namespace Unicorn.UI.Desktop.Controls
{
    /// <summary>
    /// Represents basic container for other windows controls.
    /// Initialized container also initializes all controls and containers within itself.
    /// </summary>
    public abstract class GuiContainer : GuiControl, IContainer
    {
        /// <summary>
        /// Initializes new instance of <see cref="GuiContainer"/>
        /// </summary>
        protected GuiContainer() : base()
        {
        }

        /// <summary>
        /// Initializes new instance of <see cref="GuiContainer"/> with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        protected GuiContainer(AutomationElement instance) : base(instance)
        {
        }

        /// <summary>
        /// Gets or sests control wrapped instance as <see cref="AutomationElement"/> which is also current search context.
        /// When search context was set this container is initialized by <see cref="ContainerFactory"/>
        /// </summary>
        public override AutomationElement Instance
        {
            get
            {
                if (!this.Cached)
                {
                    this.SearchContext = GetNativeControlFromParentContext(this.Locator, this.GetType());
                }

                return this.SearchContext;
            }

            set
            {
                this.SearchContext = value;
                ContainerFactory.InitContainer(this);
            }
        }

        /// <summary>
        /// Clicks button with specified name within the container.
        /// </summary>
        /// <param name="locator">button name</param>
        public virtual void ClickButton(string locator)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Click '{locator}' button");

            Button button = Find<Button>(ByLocator.Name(locator));
            button.Click();
        }

        /// <summary>
        /// Sets specified text into specified text input within the container.
        /// </summary>
        /// <param name="locator">text input name</param>
        /// <param name="text">text to input</param>
        public virtual void InputText(string locator, string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Input Text '{text}' to '{locator}' field");

            TextInput edit = Find<TextInput>(ByLocator.Name(locator));
            edit.SendKeys(text);
        }

        /// <summary>
        /// Selects specified radio button within the container.
        /// </summary>
        /// <param name="locator">radio button name</param>
        /// <returns></returns>
        public virtual bool SelectRadio(string locator)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select '{locator}' radio button");

            Radio radio = Find<Radio>(ByLocator.Name(locator));
            return radio.Select();
        }

        /// <summary>
        /// Sets specified checkbox within the container in specified state.
        /// </summary>
        /// <param name="locator">checkbox name</param>
        /// <param name="state">state to set for checkbox</param>
        /// <returns></returns>
        public virtual bool SetCheckbox(string locator, bool state)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Set checkbox '{locator}' to '{state}'");

            Checkbox checkbox = Find<Checkbox>(ByLocator.Name(locator));
            return checkbox.SetCheckedState(state);
        }
    }
}
