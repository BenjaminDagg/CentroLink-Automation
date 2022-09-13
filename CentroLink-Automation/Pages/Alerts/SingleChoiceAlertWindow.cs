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

    //Alert window with only 1 choice: OK button
    public class SingleChoiceAlertWindow : AlertWindow
    {

        private By ConfirmButton;

        public SingleChoiceAlertWindow(WindowsDriver<WindowsElement> _driver, By windowSelector) : base(_driver,windowSelector)
        {
            this.driver = _driver;

            Window = windowSelector;
            ConfirmButton = By.Name("Ok");
        }


        public void Confirm()
        {
            WindowsElement confirmBtn = (WindowsElement)wait.Until(d => d.FindElement(ConfirmButton));
            confirmBtn.Click();
        }
    }
}
