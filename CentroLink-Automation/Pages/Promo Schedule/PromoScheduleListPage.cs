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
    public class PromoScheduleListPage : DataGridPage
    {
        public override By DataGrid { get => By.ClassName("DataGrid"); }
        public override By RowSelector { get => By.ClassName("DataGridRow"); }

        ByAccessibilityId AddPromoButton;
        ByAccessibilityId EditPromoButton;
        ByAccessibilityId DeletePromoButton;
        ByAccessibilityId RefreshButton;
        ByAccessibilityId TogglePromoButton;
        MultiChoiceAlertWindow TogglePromoAlert;
        SingleChoiceAlertWindow DeletePromoAlert;
        ByAccessibilityId DaySelectorTextbox;
        ByAccessibilityId DayTextBoxIncreaseButton;
        ByAccessibilityId DayTextBoxDecreaseButton;

        public PromoScheduleListPage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            //elements
            AddPromoButton = new ByAccessibilityId("Add");
            EditPromoButton = new ByAccessibilityId("Edit");
            DeletePromoButton = new ByAccessibilityId("Delete");
            RefreshButton = new ByAccessibilityId("RefreshList");
            TogglePromoButton = new ByAccessibilityId("TogglePromoTicket");
            TogglePromoAlert = new MultiChoiceAlertWindow(driver, By.Name("Please Confirm"));
            DeletePromoAlert = new SingleChoiceAlertWindow(driver, By.Name("Message"));
            DaySelectorTextbox = new ByAccessibilityId("PART_TextBox");
            DayTextBoxIncreaseButton = new ByAccessibilityId("PART_IncreaseButton");
            DayTextBoxDecreaseButton = new ByAccessibilityId("PART_DecreaseButton");
        }


        public void EnterDayFilter(int numDays)
        {
            string numDaysString = numDays.ToString();

            driver.FindElement(DaySelectorTextbox).Clear();
            driver.FindElement(DaySelectorTextbox).SendKeys(numDaysString);
        }


        public int GetDayFilter()
        {
            string numDaysString = driver.FindElement(DaySelectorTextbox).Text;

            return int.Parse(numDaysString);
        }


        public PromoEntrySchedule GetPromoSchedule(int promoId)
        {
            WindowsElement promoList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = promoList.FindElements(RowSelector);

            foreach (var row in rows)
            {

                int id = int.Parse(row.FindElement(By.XPath(".//Custom[1]/Text")).Text);
                
                if (id == promoId)
                {
                    try
                    {
                        PromoEntrySchedule promo = new PromoEntrySchedule();
                        promo.Id = id;
                        promo.Description = row.FindElement(By.XPath(".//Custom[2]/Text")).Text;

                        string startDate = row.FindElement(By.XPath(".//Custom[3]/Text")).Text;
                        promo.StartTime = DateTime.ParseExact(startDate, "M/dd/yyyy H:mm:ss tt", CultureInfo.InvariantCulture);

                        string EndDate = row.FindElement(By.XPath(".//Custom[4]/Text")).Text;
                        promo.EndTime = DateTime.ParseExact(EndDate, "M/dd/yyyy H:mm:ss tt", CultureInfo.InvariantCulture);

                        WindowsElement startedCheckbox = (WindowsElement)row.FindElement(By.XPath(".//Custom[5]/CheckBox"));
                        promo.Started = startedCheckbox.Selected;

                        WindowsElement endedCheckbox = (WindowsElement)row.FindElement(By.XPath(".//Custom[6]/CheckBox"));
                        promo.Ended = endedCheckbox.Selected;

                        return promo;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                
            }

            return null;
        }


        public bool PromoFoundInList(int promoId)
        {
            WindowsElement promoList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = promoList.FindElements(RowSelector);

            foreach (var row in rows)
            {

                int id = int.Parse(row.FindElement(By.XPath(".//Custom[1]/Text")).Text);

                if (id == promoId)
                {
                    return true;
                }

            }

            return false;
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


        public PromoEntrySchedule SelectRowByPromoId(int promoId)
        {
            WindowsElement promoList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = promoList.FindElements(RowSelector);

            foreach (var row in rows)
            {

                int id = int.Parse(row.FindElement(By.XPath(".//Custom[1]/Text")).Text);

                if (id == promoId)
                {
                    var col = row.FindElement(By.XPath(".//Custom[1]/Text"));
                    col.Click();
                }

            }

            return null;
        }


        public void ClickAddPromo()
        {
            driver.FindElement(AddPromoButton).Click();
        }


        public void ClickEditPromo()
        {
            WindowsElement editBtn = driver.FindElement(EditPromoButton);

            if (editBtn.Enabled)
            {
                editBtn.Click();
            }
        }


        public void ClickDeletePromo()
        {
            WindowsElement deleteBtn = driver.FindElement(DeletePromoButton);

            if (deleteBtn.Enabled)
            {
                deleteBtn.Click();
            }
        }


        public void RefreshList()
        {
            driver.FindElement(RefreshButton).Click();
        }


        //If button found with text 'Turn Promo Ticket Off' then promos are already on
        //In that case do nothing. If button not found then click the toggle button
        public void TurnPromoTicketsOn()
        {
            try
            {
                driver.FindElement(By.Name("Turn Promo Ticket Off"));
                return;
            }
            catch (Exception ex)
            {
                driver.FindElement(TogglePromoButton).Click();
                TogglePromoAlert.Confirm();
            }
        }


        //If button found with text 'Turn Promo Ticket On' then promos are already off
        //In that case do nothing. If button not found then click the toggle button
        public void TurnPromoTicketsOff()
        {
            try
            {
                driver.FindElement(By.Name("Turn Promo Ticket On"));
                return;
            }
            catch (Exception ex)
            {
                driver.FindElement(TogglePromoButton).Click();
                TogglePromoAlert.Confirm();
            }
        }
    }
}
