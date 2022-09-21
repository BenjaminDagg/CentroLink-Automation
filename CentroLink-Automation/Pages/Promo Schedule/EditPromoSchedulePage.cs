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
    public class EditPromoSchedulePage : AddPromoSchedulePage
    {

        public By PromoStartedCheckbox;
        public By PromoEndedCheckbox;

        public EditPromoSchedulePage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            PromoStartedCheckbox = By.Name("Promo has already Started");
            PromoEndedCheckbox = By.Name("Promo has already Ended");
        }


        public bool PromoStarted
        {
            get
            {
                return driver.FindElement(PromoStartedCheckbox).Selected;
            }
        }


        public bool PromoEnded
        {
            get
            {
                return driver.FindElement(PromoEndedCheckbox).Selected;
            }
        }
    }
}
