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

    //Alert window containing two buttons: Yes or No
    public class MultiChoiceAlertWindow : AlertWindow
    {

        protected virtual By ConfirmButton { get; set; }
        protected virtual By CancelButton { get; set; }

        public MultiChoiceAlertWindow(WindowsDriver<WindowsElement> _driver, By windowSelector) : base(_driver,windowSelector)
        {
            this.driver = _driver;

            Window = windowSelector;
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
