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
using OpenQA.Selenium.Interactions;

namespace CentroLink_Automation
{
    public class BankSetupListPage : DataGridPage
    {

        public override By DataGrid { get => By.ClassName("DataGrid"); }
        public override By RowSelector { get => By.ClassName("DataGridRow"); }

        public ByAccessibilityId AddBankButton;
        public ByAccessibilityId EditBankButon;
        public ByAccessibilityId ViewMachinesAssignedToBankButton;
        public ByAccessibilityId RefreshButton;

        public BankSetupListPage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            AddBankButton = new ByAccessibilityId("AddBank");
            EditBankButon = new ByAccessibilityId("EditSelectedBank");
            ViewMachinesAssignedToBankButton = new ByAccessibilityId("ViewMachinesAssignedToBank");
            RefreshButton = new ByAccessibilityId("RefreshBankList");
        }


        public void ClickAddBank()
        {
            driver.FindElement(AddBankButton).Click();
        }


        public void ClickEditBank()
        {
            WindowsElement editBtn = driver.FindElement(EditBankButon);

            if (editBtn.Enabled)
            {
                editBtn.Click();
            }
        }


        public void ViewMachinesAssignedToBank()
        {
            WindowsElement viewMachBtn = driver.FindElement(ViewMachinesAssignedToBankButton);

            if (viewMachBtn.Enabled)
            {
                viewMachBtn.Click();
            }
        }


        public void Refresh()
        {
            driver.FindElement(RefreshButton).Click();
        }


        public PromoEntrySchedule SelectRowByBankId(int bankId)
        {
            WindowsElement bankList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = bankList.FindElements(RowSelector);

            foreach (var row in rows)
            {

                int id = int.Parse(row.FindElement(By.XPath(".//Custom[1]/Text")).Text);

                if (id == bankId)
                {
                    var col = row.FindElement(By.XPath(".//Custom[1]/Text"));
                    col.Click();
                }

            }

            return null;
        }


        public Bank GetBank(int bankId)
        {
            WindowsElement bankList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = bankList.FindElements(RowSelector);

            foreach (var row in rows)
            {

                int id = int.Parse(row.FindElement(By.XPath(".//Custom[1]/Text")).Text);

                if (id == bankId)
                {

                    Bank bank = new Bank();
                    bank.BankNumber = id;
                    bank.Description = row.FindElement(By.XPath(".//Custom[2]/Text")).Text;
                    bank.GameTypeCode = row.FindElement(By.XPath(".//Custom[3]/Text")).Text;

                    string IsPaper = row.FindElement(By.XPath(".//Custom[4]/Text")).Text;
                    bank.IsPaper = IsPaper == "Yes";
                    
                    bank.LockupAmount = double.Parse(row.FindElement(By.XPath(".//Custom[5]/Text")).Text, System.Globalization.NumberStyles.Currency);
                    bank.DBALockupAmount = double.Parse(row.FindElement(By.XPath(".//Custom[6]/Text")).Text, System.Globalization.NumberStyles.Currency);

                    bank.Product = row.FindElement(By.XPath(".//Custom[7]/Text")).Text;
                    bank.ProductLine = row.FindElement(By.XPath(".//Custom[8]/Text")).Text;
                    bank.PromoTicketFactor = int.Parse(row.FindElement(By.XPath(".//Custom[9]/Text")).Text);
                    bank.PromoTicketAmount = double.Parse(row.FindElement(By.XPath(".//Custom[10]/Text")).Text, System.Globalization.NumberStyles.Currency);

                    return bank;
                }

            }

            return null;
        }
    }
}
