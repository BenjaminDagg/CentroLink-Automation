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
    }
}
