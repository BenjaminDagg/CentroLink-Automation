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
    public class RemoveGameWindow : PopupWindow
    {

        protected override By Window { get => By.Name("Remove Game?"); }

        protected By ConfirmButton;
        protected By CancelButton;

        public RemoveGameWindow(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            Window = By.Name("Confirm Action");
            ConfirmButton = new ByAccessibilityId("Yes");
            CancelButton = new ByAccessibilityId("No");
        }


        public void Confirm()
        {
            WindowsElement confirmBtn = (WindowsElement)wait.Until(d => d.FindElement(ConfirmButton));
            confirmBtn.Click();
        }


        public void Cancel()
        {
            WindowsElement cancelBtn = (WindowsElement)wait.Until(d => d.FindElement(CancelButton));
            cancelBtn.Click();
        }

    }
}
