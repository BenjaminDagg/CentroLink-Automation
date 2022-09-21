using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;   //DesiredCapabilities
using System;
using Appium;
using OpenQA.Selenium.Appium;   //Appium Options
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CentroLink_Automation

{
    public abstract class BasePage
    {
        protected WindowsDriver<WindowsElement> driver;
        protected WebDriverWait wait;
        protected int DefaultWaitTimeSeconds = 5;
        
        public NavMenu NavMenu { get; set; }

        public BasePage(WindowsDriver<WindowsElement> _driver)
        {
            driver = _driver;
            wait = new WebDriverWait(driver,TimeSpan.FromSeconds(DefaultWaitTimeSeconds));

            NavMenu = new NavMenu(driver);
        }


        protected bool waitForElement(By by, int time)
        {
            int t = 0;
            while (t < time)
            {
                try
                {
                    driver.FindElement(by);
                    return true;
                }
                catch (Exception e)
                {

                }

                Thread.Sleep(1000);
                t++;
            }

            return false;
        }


        public bool ErrorIsDisplayed(By elementSelector)
        {
            try
            {
                WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(elementSelector));
                string helpText = element.GetAttribute("HelpText");

                if (string.IsNullOrEmpty(helpText))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public virtual bool IsReadOnly(By element)
        {
            return driver.FindElement(element).Enabled == false;
        }
    }
}
