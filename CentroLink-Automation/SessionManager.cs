using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;   //DesiredCapabilities
using System;
using Appium;
using OpenQA.Selenium.Appium;   //Appium Options
using System.Threading;
using OpenQA.Selenium;

namespace CentroLink_Automation
{
    static class SessionManager
    {
        private static WindowsDriver<WindowsElement> driver;

        public static WindowsDriver<WindowsElement> Driver
        {
            get
            {
                if (driver == null)
                {
                    throw new NullReferenceException("Driver has not been initialized. Call Init before referencing Driver.");
                }

                return driver;
            }
        }

        public static void Init()
        {
            if (driver == null)
            {
                string DriverUrl = "http://127.0.0.1:4723";         //found by starting WinAppDriver.exe
                string AppPath = @"C:\Program Files (x86)\Diamond Game Enterprises\CentroLink\CentroLink.exe";
                string AppDriverPath = @"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe";

                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", AppPath);
                appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");

                driver = new WindowsDriver<WindowsElement>(new Uri(DriverUrl), appiumOptions);

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            }

        }


        public static void Close()
        {


            //Close driver (app)
            if (driver != null)
            {
                driver.Close();
                driver.Quit();


            }

            driver = null;
        }
    }
}
