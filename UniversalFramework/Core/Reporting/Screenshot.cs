﻿using Core.Logging;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace Core.Reporting
{
    public class Screenshot
    {
        public static Bitmap PrintScreen(string fileName)
        {
            try
            {
                Logger.Info("Creating print screen...");
                int screenWidth = Screen.PrimaryScreen.Bounds.Width;
                int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                Bitmap printscreen = new Bitmap(screenWidth, screenHeight);
                Graphics graphics = Graphics.FromImage(printscreen as Image);
                graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
                return printscreen;
//                printscreen.Save(string.Format("{0}{1}.{2}", FILEPATH, fileName, ImageFormat.Jpeg.ToString()), ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Logger.Info("Failed to get/save print screen...");
                Logger.Info("Exception: " + e.ToString());
                return null;
            }
        }
    }
}
