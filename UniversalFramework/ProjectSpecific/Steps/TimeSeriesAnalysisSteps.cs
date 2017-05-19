﻿using Core.Testing;
using Core.Testing.Attributes;
using System.Threading;
using UICore.Driver;
using UIDesktop.Driver;
using UIDesktop.UI.Controls;

namespace ProjectSpecific.Steps
{
    public class TimeSeriesAnalysisSteps : TestSteps
    {
        IDriver driver;


        [TestStep("Start Time Series Analysis '{0}'")]
        public void StartApplication(string value)
        {
            ReportStep(value);

            driver = GuiDriver.Instance;
            driver.Get(value);
        }

        [TestStep("Open File '{0}' For Analysis")]
        public void OpenFile(string fileName)
        {
            ReportStep(fileName);

            Window mainWindow = driver.WaitForElement<Window>("mainForm");
            mainWindow.ClickButton("openFileBtn");

            Window openDialog = mainWindow.WaitForElement<Window>("Open");
            openDialog.InputText("File name:", fileName);
            openDialog.ClickButton("Open");
            Thread.Sleep(2000);
        }



        [TestStep("Draw charts")]
        public void DrawCharts()
        {
            ReportStep();

            Window mainWindow = driver.WaitForElement<Window>("mainForm");
            mainWindow.ClickButton("plotBtn");
        }


        [TestStep("Close Time Series Analysis")]
        public void CloseApplication()
        {
            ReportStep();

            driver.Close();
        }
    }
}
