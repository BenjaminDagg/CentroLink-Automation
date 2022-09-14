using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;   //DesiredCapabilities
using System;
using Appium;
using OpenQA.Selenium.Appium;   //Appium Options
using System.Threading;
using OpenQA.Selenium;
using System.Linq;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;

namespace CentroLink_Automation
{
    public enum MenuIndex : int
    {
        Maintenance = 1,
        MachineSetup = 2,
        DealStatus = 3,
        MachinesInUse = 4,
        LocationSetup = 5,
        PromoTicketSetup = 6,
        BankSetup = 7,
        Settings = 8,
        UserAdmin = 9,
        ChangePassword = 10,
        About = 11,
        Logout = 12
    }

    public class NavMenu
    {
        

        public By MaintenanceTab;
        public By SettingsTab;
        private int MaintenanceTabChildCount = 7;
        private int SettingsTabChildCount = 3;
        public By MachineSetupTab;
        public By DealStatusTab;
        WebDriverWait wait;
        private int WaitTimeoutSec = 5;
        
        private WindowsDriver<WindowsElement> driver;

        public NavMenu(WindowsDriver<WindowsElement> _driver)
        {
            this.driver = _driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(WaitTimeoutSec));

            MaintenanceTab = By.XPath("(//Window[@ClassName='Window'][@Name='CentroLink']/Custom[@ClassName='MainContentView']/Button[@ClassName='Button'])[1]");
            SettingsTab = By.XPath("(//Window[@ClassName='Window'][@Name='CentroLink']/Custom[@ClassName='MainContentView']/Button[@ClassName='Button'])[9]");
            MachineSetupTab = By.XPath("(//Window[@ClassName='Window'][@Name='CentroLink']/Custom[@ClassName='MainContentView']/Button[@ClassName='Button'])[2]");
            DealStatusTab = By.XPath("(//Window[@ClassName='Window'][@Name='CentroLink']/Custom[@ClassName='MainContentView']/Button[@ClassName='Button'])[4]");
        }


        public void ClickMaintenanceTab()
        {
            
            driver.FindElement(MaintenanceTab).Click();
        }


        public void ClickSettingsTab()
        {
            driver.FindElement(SettingsTab).Click();
        }


        public void ExpandAll()
        {
            if(MenuIsExpanded() == false)
            {
                ClickMaintenanceTab();
                ClickSettingsTab();
            }
        }


        public bool MenuIsExpanded()
        {
            
            
            wait.Until(d => d.FindElements(By.XPath("//Window[@ClassName='Window'][@Name='CentroLink']/Custom[@ClassName='MainContentView']/Button[@ClassName='Button']")).Count > 0);
            int count =  driver.FindElements(By.XPath("//Window[@ClassName='Window'][@Name='CentroLink']/Custom[@ClassName='MainContentView']/Button[@ClassName='Button']")).Count;

            return count == 13;
        }


        public void ClickMachineSetupTab()
        {
            ExpandAll();

            WindowsElement machineTab = (WindowsElement)wait.Until(d => d.FindElement(MachineSetupTab));
            machineTab.Click();
        }


        public void ClickDealStatusTab()
        {
            ExpandAll();

            WindowsElement dealTab = (WindowsElement)wait.Until(d => d.FindElement(DealStatusTab));
            dealTab.Click();
        }
    }
}
