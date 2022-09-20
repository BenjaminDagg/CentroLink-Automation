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
    public class AddLocationPage : EditLocationPage
    {
        public AddLocationPage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;
        }


        public void EnterLocationId(string text)
        {
            driver.FindElement(LocationIdField).Clear();
            driver.FindElement(LocationIdField).SendKeys(text);
        }


        public void EnterDgeId(string text)
        {
            driver.FindElement(DgeIdField).Clear();
            driver.FindElement(DgeIdField).SendKeys(text);
        }


        public void EnterRetailerNuber(string text)
        {
            driver.FindElement(RetailerNumberField).Clear();
            driver.FindElement(RetailerNumberField).SendKeys(text);
        }


        public void EnterForm(string locName, string address1, string address2, string city, 
            TPISetting TPI, string state, string postalCode, string phone, string cashoutTimeout, 
            string maxBalanceAdjustment, string fax, string payoutAuthorizationAmount, string sweepAmount,
            string locationId, string dgeId, string retailerNumber)
        {
            base.EnterForm(
                locName, 
                address1, 
                address2, 
                city, 
                TPI, 
                state, 
                postalCode, 
                phone, 
                cashoutTimeout, 
                maxBalanceAdjustment, 
                fax, 
                payoutAuthorizationAmount, 
                sweepAmount
            );
            EnterLocationId(locationId);
            EnterDgeId(dgeId);
            EnterRetailerNuber(retailerNumber);
        }
    }
}
