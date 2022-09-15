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

/*
 * Machine is in use if:
 *  - balance > 0 AND
 *  - last play is set (sent W or L trans to TP or manually insert record to MACH_LAST_PLAY)
 */

namespace CentroLink_Automation.Pages
{
    public class MachinesInUsePage : DataGridPage
    {
        public override By DataGrid { get => By.ClassName("DataGrid"); }
        public override By RowSelector { get => By.ClassName("DataGridRow");}

        public MachinesInUsePage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;
        }
    }
}
