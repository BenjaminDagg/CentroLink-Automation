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
    public class EditLocationPage : BasePage
    {

        public By LocationNameField;
        public By Address1Field;
        public By Address2Field;
        public By CityField;
        public By StateField;
        public By PostalCodeField;
        public By PhoneField;
        public By FaxField;
        public By SweepAmountField;
        public By LocationIdField;
        public By DgeIdField;
        public By RetailerNumberField;
        public DropdownElement TpiDropdown { get; set; }
        public By CashoutTimeoutField;
        public By MaxBalanceAdjustmentField;
        public By PayoutAuthorizationAmount;

        public By DefaultCheckbox;
        public By JackpotLockupCheckbox;
        public By PrintPromoTicketsCheckbox;
        public By AllowTicketReprintCheckbox;
        public By SummarizePlayForHoldByDenomReportCheckbox;
        public By AutoDropOnCashDoorCheckbox;

        private ByAccessibilityId SaveButton;
        private ByAccessibilityId BackButton;

        public MultiChoiceAlertWindow ConfirmationPrompt;
        public SingleChoiceAlertWindow SuccessAlert;
        public SingleChoiceAlertWindow DuplicateErrorAlert;
        public enum TPISetting  { 
            DiamondGameBackOffice = 0, 
            SierraDesignGroup = 1, 
            MultiMediaGamesInc = 2, 
            IowaStateLottery = 3, 
            SlotAccountingSystem = 4,
            None = 5
        }

        public EditLocationPage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            //text fields
            LocationNameField = By.XPath("//Edit[1]");
            LocationIdField = By.XPath("//Edit[2]");
            Address1Field = By.XPath("//Edit[3]");
            DgeIdField = By.XPath("//Edit[4]");
            Address2Field = By.XPath("//Edit[5]");
            RetailerNumberField = By.XPath("//Edit[6]");
            CityField = By.XPath("//Edit[7]");
            StateField = By.XPath("//Edit[9]");
            PostalCodeField = By.XPath("//Edit[10]");
            CashoutTimeoutField = By.XPath("//Edit[8]");
            PhoneField = By.XPath("//Edit[11]");
            MaxBalanceAdjustmentField = By.XPath("//Edit[12]");
            FaxField = By.XPath("//Edit[13]");
            PayoutAuthorizationAmount = By.XPath("//Edit[14]");
            SweepAmountField = By.XPath("//Edit[15]");
            
            TpiDropdown = new DropdownElement(By.ClassName("ComboBox"),driver);

            //Checkboxes
            DefaultCheckbox = By.XPath("//CheckBox/Text[@Name='Set Location As Default']/parent::CheckBox");
            JackpotLockupCheckbox = By.XPath("//CheckBox/Text[@Name='Jackpot Lockup']");
            PrintPromoTicketsCheckbox = By.XPath("//CheckBox/Text[@Name='Print Promo Tickets']");
            AllowTicketReprintCheckbox = By.XPath("//CheckBox/Text[@Name='Allow Ticket Reprint']");
            SummarizePlayForHoldByDenomReportCheckbox = By.XPath("//CheckBox/Text[@Name='Summarize Play For Hold By Denom Report']");
            AutoDropOnCashDoorCheckbox = By.XPath("//CheckBox/Text[@Name='Auto Drop on Cash Door Open']");

            //buttons
            SaveButton = new ByAccessibilityId("Save");
            BackButton = new ByAccessibilityId("BackToLocationSetup");

            //alerts
            SuccessAlert = new SingleChoiceAlertWindow(driver, By.Name("Success"));
            ConfirmationPrompt = new MultiChoiceAlertWindow(driver, By.Name("Confirm Action"));
            DuplicateErrorAlert = new SingleChoiceAlertWindow(driver, By.Name("Error"));
        }


        public void EnterLocationName(string text)
        {
            driver.FindElement(LocationNameField).Clear();
            driver.FindElement(LocationNameField).SendKeys(text);
        }


        public string GetLocationName()
        {
            return driver.FindElement(LocationNameField).Text;
        }


        public string GetLocationId()
        {
            return driver.FindElement(LocationIdField).Text;
        }


        public void EnterAddress1(string text)
        {
            driver.FindElement(Address1Field).Clear();
            driver.FindElement(Address1Field).SendKeys(text);
        }


        public string GetAddress1()
        {
            return driver.FindElement(Address1Field).Text;
        }


        public string GetDgeId()
        {
            return driver.FindElement(DgeIdField).Text;
        }

        public void EnterAddress2(string text)
        {
            driver.FindElement(Address2Field).Clear();
            driver.FindElement(Address2Field).SendKeys(text);
        }


        public string GetAddress2()
        {
            return driver.FindElement(Address2Field).Text;
        }


        public string GetRetailnumber()
        {
            return driver.FindElement(RetailerNumberField).Text;
        }

        public void EnterCity(string text)
        {
            driver.FindElement(CityField).Clear();
            driver.FindElement(CityField).SendKeys(text);
        }


        public string GetCity()
        {
            return driver.FindElement(CityField).Text;
        }

        public void EnterState(string text)
        {
            driver.FindElement(StateField).Clear();
            driver.FindElement(StateField).SendKeys(text);
        }


        public string GetState()
        {
            return driver.FindElement(StateField).Text;
        }


        public void EnterPostalCode(string text)
        {
            driver.FindElement(PostalCodeField).Clear();
            driver.FindElement(PostalCodeField).SendKeys(text);
        }


        public string GetPostalCode()
        {
            return driver.FindElement(PostalCodeField).Text;
        }


        public void EnterCashoutTimeout(string text)
        {
            driver.FindElement(CashoutTimeoutField).Clear();
            driver.FindElement(CashoutTimeoutField).SendKeys(text);
        }


        public string GetCashoutTimeout()
        {
            return driver.FindElement(CashoutTimeoutField).Text;
        }

        public void EnterPhone(string text)
        {
            driver.FindElement(PhoneField).Clear();
            driver.FindElement(PhoneField).SendKeys(text);
        }


        public string GetPhone()
        {
            return driver.FindElement(PhoneField).Text;
        }


        public void EnterMaxBetAdjustment(string text)
        {
            driver.FindElement(MaxBalanceAdjustmentField).Clear();
            driver.FindElement(MaxBalanceAdjustmentField).SendKeys(text);
        }


        public string GetMaxBalanceAdjustment()
        {
            return driver.FindElement(MaxBalanceAdjustmentField).Text;
        }


        public void EnterFax(string text)
        {
            driver.FindElement(FaxField).Clear();
            driver.FindElement(FaxField).SendKeys(text);
        }


        public string GetFax()
        {
            return driver.FindElement(FaxField).Text;
        }


        public void EnterPayoutAuthorizationAmount(string text)
        {
            driver.FindElement(PayoutAuthorizationAmount).Clear();
            driver.FindElement(PayoutAuthorizationAmount).SendKeys(text);
        }


        public string GetPayoutAuthorizationAmount()
        {
            return driver.FindElement(PayoutAuthorizationAmount).Text;
        }


        public void EnterSweepAmount(string text)
        {
            driver.FindElement(SweepAmountField).Clear();
            driver.FindElement(SweepAmountField).SendKeys(text);
        }


        public string GetSweepAmount()
        {
            return driver.FindElement(SweepAmountField).Text;
        }


        public void SetLocationAsDefault(bool enabled)
        {
            WindowsElement checkbox = driver.FindElement(DefaultCheckbox);

            if(checkbox.Selected != enabled)
            {
                checkbox.Click();
            }
        }


        public void SetJackpotLockupEnabled(bool enabled)
        {
            WindowsElement checkbox = driver.FindElement(JackpotLockupCheckbox);

            if (checkbox.Selected != enabled)
            {
                checkbox.Click();
            }
        }


        public void SetPrintPromoTicketEnabled(bool enabled)
        {
            WindowsElement checkbox = driver.FindElement(PrintPromoTicketsCheckbox);

            if (checkbox.Selected != enabled)
            {
                checkbox.Click();
            }
        }


        public void SetAllowTicketReprintEnabled(bool enabled)
        {
            WindowsElement checkbox = driver.FindElement(AllowTicketReprintCheckbox);

            if (checkbox.Selected != enabled)
            {
                checkbox.Click();
            }
        }


        public void SetSummarizePlayHoldEnabled(bool enabled)
        {
            WindowsElement checkbox = driver.FindElement(SummarizePlayForHoldByDenomReportCheckbox);

            if (checkbox.Selected != enabled)
            {
                checkbox.Click();
            }
        }


        public void SetAutoCashDropEnabled(bool enabled)
        {
            WindowsElement checkbox = driver.FindElement(AutoDropOnCashDoorCheckbox);

            if (checkbox.Selected != enabled)
            {
                checkbox.Click();
            }
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


        public void CancelChanges()
        {
            ClickBackButton();
            ConfirmationPrompt.Confirm();
        }


        public void SelectTPI(EditLocationPage.TPISetting selection)
        {

            if(selection != EditLocationPage.TPISetting.None)
            {
                int index = (int)selection;
                TpiDropdown.SelectByIndex(index);
            }
            
        }


        public virtual void EnterForm(string locName,string address1, string address2, string city, 
            EditLocationPage.TPISetting TPI, string state, string postalCode, string phone, string cashoutTimeout, 
            string maxBalanceAdjustment, string fax, string payoutAuthorizationAmount, string sweepAmount)
        {
            EnterLocationName(locName);
            EnterAddress1(address1);
            EnterAddress2(address2);
            EnterCity(city);
            SelectTPI(TPI);
            EnterState(state);
            EnterPostalCode(postalCode);
            EnterCashoutTimeout(cashoutTimeout);
            EnterPhone(phone);
            EnterMaxBetAdjustment(maxBalanceAdjustment);
            EnterFax(fax);
            EnterPayoutAuthorizationAmount(payoutAuthorizationAmount);
            EnterSweepAmount(sweepAmount);
        }
        

        //If element is read only it throws an error when trying to type in it with SendKeys
        public bool IsReadOnly(By element)
        {
            try
            {
                driver.FindElement(element).SendKeys("test");
                return false;
            }
            catch (Exception e)
            {
                return true;
            }
        }

    }
}
