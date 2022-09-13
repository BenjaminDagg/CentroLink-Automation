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
    public class PopupWindow
    {
        protected WindowsDriver<WindowsElement> driver;
        protected DefaultWait<WindowsDriver<WindowsElement>> wait;
        protected int WAIT_TIMEOUT_SEC = 5;
        protected virtual By Window { get; set; }
        protected virtual By CloseButton { get; set; }

        public PopupWindow(WindowsDriver<WindowsElement> _driver)
        {
            this.driver = _driver;
            wait = new DefaultWait<WindowsDriver<WindowsElement>>(driver);
            wait.Timeout = TimeSpan.FromSeconds(5);

            Window = By.Name("Confirm Action");
        }


        public virtual bool IsOpen
        {
            get
            {
                try
                {

                    Thread.Sleep(1000);
                    wait.Until(d =>
                    {
                        WindowsElement element = d.FindElement(Window);

                        return element != null;
                    });
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }


        protected virtual void WaitForWindowOpen()
        {
            try
            {
                wait.Until(d => d.FindElement(Window));
                
            }
            catch (Exception ex)
            {

            }
        }
    }
}
