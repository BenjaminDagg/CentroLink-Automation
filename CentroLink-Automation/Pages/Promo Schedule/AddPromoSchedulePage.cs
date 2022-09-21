using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;   //DesiredCapabilities
using System;
using Appium;
using OpenQA.Selenium.Appium;   //Appium Options
using System.Threading;
using OpenQA.Selenium;
using System.Linq;
using System.Collections.Generic;
using CentroLink_Automation.Pages.Machine_Setup;
using System.Globalization;

namespace CentroLink_Automation
{
    public class AddPromoSchedulePage : BasePage
    {
        public By Description;
        public By StartDateField;
        public By EndDateField;

        public ByAccessibilityId SaveButton;
        public ByAccessibilityId BackButton;

        public MultiChoiceAlertWindow ConfirmationWindow;
        public SingleChoiceAlertWindow SuccessAlert;

        public AddPromoSchedulePage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            Description = By.ClassName("TextBox");
            StartDateField = By.XPath("(//Edit[@AutomationId = 'PART_TextBox'])[1]");
            EndDateField = By.XPath("(//Edit[@AutomationId = 'PART_TextBox'])[2]");

            SaveButton = new ByAccessibilityId("Save");
            BackButton = new ByAccessibilityId("BackToPromoTicketSetup");

            ConfirmationWindow = new MultiChoiceAlertWindow(driver, By.Name("Confirm Action"));
            SuccessAlert = new SingleChoiceAlertWindow(driver, By.Name("Success"));
        }


        public void EnterDescription(string text)
        {
            driver.FindElement(Description).Clear();
            driver.FindElement(Description).SendKeys(text);
        }


        public string GetDescription()
        {
            return driver.FindElement(Description).Text;
        }


        public void EnterPromoStartDate(string text)
        {
            driver.FindElement(StartDateField).SendKeys(Keys.Control + "a");
            driver.FindElement(StartDateField).SendKeys(Keys.Backspace);
            driver.FindElement(StartDateField).SendKeys(text);
            driver.FindElement(StartDateField).SendKeys(Keys.Enter);
        }


        public string GetPromoStartDate()
        {
            return driver.FindElement(StartDateField).Text;
        }


        public void EnterPromoEndDate(string text)
        {
            driver.FindElement(EndDateField).SendKeys(Keys.Control + "a");
            driver.FindElement(EndDateField).SendKeys(Keys.Backspace);
            driver.FindElement(EndDateField).SendKeys(text);
            driver.FindElement(StartDateField).SendKeys(Keys.Enter);
        }


        public string GetPromoEndDate()
        {
            return driver.FindElement(EndDateField).Text;
        }


        public void ClickSave()
        {
            driver.FindElement(SaveButton).Click();
        }


        public void Save()
        {
            ClickSave();
            SuccessAlert.Confirm();
        }


        public void ClickBackButton()
        {
            driver.FindElement(BackButton).Click();
        }


        public void ReturnToPromoList()
        {
            ClickBackButton();
            ConfirmationWindow.Confirm();
        }
    }
}
