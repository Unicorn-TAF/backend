﻿using System;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Core.PageObject;

namespace Unicorn.UI.Mobile.Android.Controls
{
    public class AndroidContainer : AndroidControl, IContainer
    {
        public AndroidContainer() : base()
        {
        }

        public AndroidContainer(AppiumWebElement instance) : base(instance)
        {
        }

        public override AppiumWebElement Instance
        {
            get
            {
                if (!this.Cached)
                {
                    this.SearchContext = GetNativeControlFromParentContext(this.Locator);
                }

                return this.SearchContext;
            }

            set
            {
                this.SearchContext = value;
                ContainerFactory.InitContainer(this);
            }
        }

        public void ClickButton(string locator)
        {
            throw new NotImplementedException();
        }

        public void InputText(string locator, string text)
        {
            throw new NotImplementedException();
        }

        public bool SelectRadio(string locator)
        {
            throw new NotImplementedException();
        }

        public bool SetCheckbox(string locator, bool state)
        {
            throw new NotImplementedException();
        }
    }
}
