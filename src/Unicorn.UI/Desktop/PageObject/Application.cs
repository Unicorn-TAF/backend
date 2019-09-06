﻿using System;
using System.Diagnostics;
using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.Controls;
using Unicorn.UI.Desktop.Controls.Typified;
using Unicorn.UI.Desktop.Driver;

namespace Unicorn.UI.Desktop.PageObject
{
    /// <summary>
    /// Represents base of windows application. Contains fields related to paths, process, methods for starting and closing the application.
    /// </summary>
    public abstract class Application : GuiContainer
    {
        /// <summary>
        /// Initializas new instance of <see cref="Application"/> located in specified directory and with specified exe name.
        /// </summary>
        /// <param name="path">path to application</param>
        /// <param name="exeName">.exe file name</param>
        protected Application(string path, string exeName)
        {
            this.SearchContext = GuiDriver.Instance.SearchContext;
            ContainerFactory.InitContainer(this);
            this.Path = path;
            this.ExeName = exeName;
        }

        /// <summary>
        /// Gets app root control type (by default it's Pane as root of windows desktop).
        /// </summary>
        public override ControlType UiaType => ControlType.Pane;

        /// <summary>
        /// Gets or sets path to application.
        /// </summary>
        public string Path { get; protected set; }

        /// <summary>
        /// Gets or sets application exe file name.
        /// </summary>
        public string ExeName { get; protected set; }

        /// <summary>
        /// Gets or sets Process of application instance.
        /// </summary>
        public Process Process { get; protected set; }

        /// <summary>
        /// Start application and assign process to started instance.
        /// </summary>
        public virtual void Start()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Start {this.ExeName} application");
            this.Process = Process.Start(System.IO.Path.Combine(this.Path, this.ExeName));
        }

        /// <summary>
        /// Close opened application instance.
        /// </summary>
        public virtual void Close()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Close {this.ExeName} application");
            try
            {
                new Window(AutomationElement.FromHandle(this.Process.MainWindowHandle)).Close();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, $"Unable to close {this.ExeName} application: {ex.Message}");
            }
        }
    }
}
