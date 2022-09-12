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
    public class ErrorWindow : PopupWindow
    {

        protected override By Window { get => By.Name("Error");}
        private By ConfirmButton;

        public ErrorWindow(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            Window = By.Name("Error");
            ConfirmButton = By.Name("Ok");
        }


        public void Confirm()
        {
            WindowsElement confirmBtn = (WindowsElement)wait.Until(d => d.FindElement(ConfirmButton));
            confirmBtn.Click();
        }
    }
}
