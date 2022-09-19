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
    public class LocationSetupPage : DataGridPage
    {
        public ByAccessibilityId AddLocationButton;
        public ByAccessibilityId EditLocationButton;
        private ByAccessibilityId RefreshButton;
        public override By DataGrid { get => By.ClassName("DataGrid"); }
        public override By RowSelector { get => By.ClassName("DataGridRow"); }

        public LocationSetupPage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            //elements
            AddLocationButton = new ByAccessibilityId("Add");
            EditLocationButton = new ByAccessibilityId("Edit");
            RefreshButton = new ByAccessibilityId("RefreshList");
        }


        public void ClickEditLocation()
        {
            driver.FindElement(EditLocationButton).Click();
        }


        public void Refresh()
        {
            driver.FindElement(RefreshButton).Click();
        }


        public Location GetLocation()
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var row = machineList.FindElement(RowSelector);

            Location location = new Location();

            try
            {
                location.DgeId = row.FindElement(By.XPath(".//Custom[1]/Text")).Text;
                location.LocationId = int.Parse(row.FindElement(By.XPath(".//Custom[2]/Text")).Text);
                location.LocationName = row.FindElement(By.XPath(".//Custom[3]/Text")).Text;
                location.RetailerNumber = int.Parse(row.FindElement(By.XPath(".//Custom[4]/Text")).Text);

                string defaultText = row.FindElement(By.XPath(".//Custom[5]/Text")).Text;
                location.IsDefault = defaultText == "True";

                string startDate = row.FindElement(By.XPath(".//Custom[6]/Text")).Text;
                location.AccountDayStart = DateTime.ParseExact(startDate, "HH:mm:ss", CultureInfo.InvariantCulture);

                string endDate = row.FindElement(By.XPath(".//Custom[7]/Text")).Text;
                location.AccountDayEnd = DateTime.ParseExact(endDate, "HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return location;
        }


        public override void SelectRow(int rowNum)
        {
            WindowsElement list = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = list.FindElements(By.ClassName("DataGridRow"));

            try
            {
                var row = list.FindElement(By.XPath("(.//DataItem[@ClassName='DataGridRow'])[" + (rowNum + 1) + "]/Custom[1]/Text"));
                row.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetMachine: Invalid row " + rowNum);
            }
        }
    }
}
