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
    public class DealStatusPage : DataGridPage
    {

        public By DealStatusDropdownSelector;
        public DropdownElement DealStatusDropdown;
        public ByAccessibilityId CloseDealButton;
        public MultiChoiceAlertWindow ConfirmationWindow;
        public SingleChoiceAlertWindow SuccessAlert;
        public ByAccessibilityId RefreshButton;
        public enum DealStatus { RecommendedForClose = 0, Open = 1, Closed = 2 , All = 3 }
        public override By DataGrid { get => By.ClassName("DataGrid"); }
        public override By RowSelector { get => By.ClassName("DataGridRow"); }

        public DealStatusPage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            DealStatusDropdownSelector = By.ClassName("ComboBox");
            DealStatusDropdown = new DropdownElement(DealStatusDropdownSelector,driver);
            CloseDealButton = new ByAccessibilityId("CloseDeals");
            ConfirmationWindow = new MultiChoiceAlertWindow(driver, By.Name("Confirm Close Deal(s)"));
            RefreshButton = new ByAccessibilityId("RefreshList");
            SuccessAlert = new SingleChoiceAlertWindow(driver, By.Name("Deal Close Status"));
        }


        public void SelectDealStatusFilter(DealStatus filter)
        {
            int index = (int)filter;
            
            DealStatusDropdown.SelectByIndex(index);
        }


        public DealStatus GetSelectedFilter()
        {
            string currentFilter = DealStatusDropdown.SelectedOption;

            switch (currentFilter)
            {
                case "All Deals":
                    return DealStatus.All;
                case "All Open Deals":
                    return DealStatus.Open;
                case "All Closed Deals":
                    return DealStatus.Closed;
                case "Deals Recommended For Closing":
                    return DealStatus.RecommendedForClose;
                default:
                    return DealStatus.All;
            }
        }


        public List<DealStatus> DealStatusFilterOptions()
        {
            var options = DealStatusDropdown.Options;
            List<DealStatus> result = new List<DealStatus>();

            foreach (var option in options)
            {
                switch (option)
                {
                    case "All Deals":
                        result.Add(DealStatus.All);
                        break;
                    case "All Open Deals":
                        result.Add(DealStatus.Open);
                        break;
                    case "All Closed Deals":
                        result.Add(DealStatus.Closed);
                        break;
                    case "Deals Recommended For Closing":
                        result.Add(DealStatus.RecommendedForClose);
                        break;
                    default:
                        result.Add(DealStatus.All);
                        break;
                }
            }

            return result;
        }


        public void SelectRowByDealNumber(int dealNum)
        {
            WindowsElement dealList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = dealList.FindElements(RowSelector);

            foreach (var row in rows)
            {
                var dealCol = row.FindElement(By.XPath("(.//Custom[@ClassName='DataGridCell'])[1]"));
                int dealVal = int.Parse(dealCol.Text);

                if(dealVal == dealNum)
                {
                    dealCol.Click();
                }
            }
        }


        public bool DealFoundInList(int dealNum)
        {
            WindowsElement dealList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = dealList.FindElements(RowSelector);

            foreach (var row in rows)
            {
                var dealCol = row.FindElement(By.XPath("(.//Custom[@ClassName='DataGridCell'])[1]"));
                int dealVal = int.Parse(dealCol.Text);

                if (dealVal == dealNum)
                {
                    return true;
                }
            }

            return false;
        }


        public override void SelectRows(params int[] rowNums)
        {
            WindowsElement list = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = list.FindElements(By.ClassName("DataGridRow"));

            Actions holdKeyAction = new Actions(driver);
            holdKeyAction.KeyDown(Keys.Control);
            holdKeyAction.Perform();

            Actions keyUpAction = new Actions(driver);
            keyUpAction.KeyUp(Keys.Control);

            foreach (int rowNum in rowNums)
            {
                try
                {
  
                    var item = list.FindElement(By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom[1]"));
                    item.Click();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetMachine: Invalid row " + rowNum);
                }
            }

            keyUpAction.Perform();
        }


        private bool CloseDealIsEnabled()
        {
            return driver.FindElement(CloseDealButton).Enabled;
        }


        public void ClickCloseDealButton()
        {
            if(CloseDealIsEnabled() == true)
            {
                driver.FindElement(CloseDealButton).Click();
            }
        }


        public void Refresh()
        {
            driver.FindElement(RefreshButton).Click();
        }


        public void CloseDeal()
        {
            if (CloseDealIsEnabled() == true)
            {
                driver.FindElement(CloseDealButton).Click();
                ConfirmationWindow.Confirm();

                SuccessAlert.Confirm();

            }
        }


        public Deal GetDealAtRowNum(int rowNum)
        {
            WindowsElement list = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            Deal deal = new Deal();
            
            try
            {
                var row = list.FindElement(By.XPath(".//DataItem[" + (rowNum + 1) + "]"));
                deal.DealNumber = int.Parse(row.FindElement(By.XPath(".//Custom[1]")).Text);

                string status = row.FindElement(By.XPath(".//Custom[2]")).Text;
                deal.IsOpen = status == "Open";
                
                string recommendClose = row.FindElement(By.XPath(".//Custom[3]")).Text;
                deal.RecommendedToClose = recommendClose == "Yes";

                deal.Description = row.FindElement(By.XPath(".//Custom[4]")).Text;
                deal.TabAmount = double.Parse(row.FindElement(By.XPath(".//Custom[5]")).Text, System.Globalization.NumberStyles.Currency); ;
                deal.TabsDispensed = int.Parse(row.FindElement(By.XPath(".//Custom[6]")).Text);
                deal.TabsPerDeal = int.Parse(row.FindElement(By.XPath(".//Custom[7]")).Text);
                deal.Completed = double.Parse(row.FindElement(By.XPath(".//Custom[8]")).Text.Replace("%", ""));
                deal.DealOpen = DateTime.Parse(row.FindElement(By.XPath(".//Custom[9]")).Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetMachine: Invalid row " + rowNum);
            }

            return deal;
        }


        public Deal GetDealByDealNumber(int dealNum)
        {
            WindowsElement dealList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = dealList.FindElements(RowSelector);

            Deal deal = new Deal();

            foreach (var row in rows)
            {
                var dealCol = row.FindElement(By.XPath("(.//Custom[@ClassName='DataGridCell'])[1]"));
                int dealVal = int.Parse(dealCol.Text);

                if (dealVal == dealNum)
                {
                    deal.DealNumber = int.Parse(row.FindElement(By.XPath(".//Custom[1]")).Text);

                    string status = row.FindElement(By.XPath(".//Custom[2]")).Text;
                    deal.IsOpen = status == "Open";

                    string recommendClose = row.FindElement(By.XPath(".//Custom[3]")).Text;
                    deal.RecommendedToClose = recommendClose == "Yes";

                    deal.Description = row.FindElement(By.XPath(".//Custom[4]")).Text;
                    deal.TabAmount = double.Parse(row.FindElement(By.XPath(".//Custom[5]")).Text, System.Globalization.NumberStyles.Currency); ;
                    deal.TabsDispensed = int.Parse(row.FindElement(By.XPath(".//Custom[6]")).Text);
                    deal.TabsPerDeal = int.Parse(row.FindElement(By.XPath(".//Custom[7]")).Text);
                    deal.Completed = double.Parse(row.FindElement(By.XPath(".//Custom[8]")).Text.Replace("%", ""));
                    deal.DealOpen = DateTime.Parse(row.FindElement(By.XPath(".//Custom[9]")).Text);
                }
            }

            return deal;
        }
    }
}
