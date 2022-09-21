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
    }
}
