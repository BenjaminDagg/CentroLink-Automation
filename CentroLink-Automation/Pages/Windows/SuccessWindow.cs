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
    public class SuccessWindow : PopupWindow
    {
        private By ConfirmButton;

        protected override By Window { get => By.Name("Success");}

        public SuccessWindow(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            Window = By.Name("Success");
            ConfirmButton = new ByAccessibilityId("Ok");
        }


        public void Confirm()
        {
            WindowsElement confirmBtn = (WindowsElement)wait.Until(d => d.FindElement(ConfirmButton));
            confirmBtn.Click();
        }
    }
}
