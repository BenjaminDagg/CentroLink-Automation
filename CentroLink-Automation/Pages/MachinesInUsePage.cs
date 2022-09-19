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

namespace CentroLink_Automation
{
    public class MachinesInUsePage : DataGridPage
    {
        public override By DataGrid { get => By.ClassName("DataGrid"); }
        public override By RowSelector { get => By.ClassName("DataGridRow");}
        public By LastRefreshTimestamp;

        public MachinesInUsePage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            LastRefreshTimestamp = By.XPath("//Text[contains(@Name, 'Last Refreshed')]");
        }


        public string LastRefresh
        {
            get
            {
                return driver.FindElement(LastRefreshTimestamp).Text;
            }
        }


        public bool MachineFoundInList(string machineNumber)
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = machineList.FindElements(RowSelector);

            foreach (var row in rows)
            {
                string machineNo = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text;

                if (machineNo == machineNumber)
                {
                    return true;
                }
            }

            return false;
        }


        public bool MachineFoundInList(int machineNumber)
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = machineList.FindElements(RowSelector);

            foreach (var row in rows)
            {
                int machineNo = int.Parse(row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text);
                
                if (machineNo == machineNumber)
                {
                    return true;
                }
            }

            return false;
        }


        public Machine GetMachineAtRow(int rowNum)
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = machineList.FindElements(RowSelector);

            try
            {
                var row = machineList.FindElement(By.XPath("(.//DataItem[@ClassName='DataGridRow'])[" + (rowNum + 1) + "]"));

                Machine newMachine = new Machine();

                newMachine.MachineNumber = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text;
                string statusText = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[2]")).Text;
                newMachine.Status = statusText == "Online";
                newMachine.Description = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[3]")).Text;
                newMachine.Balance = double.Parse(row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[4]")).Text, System.Globalization.NumberStyles.Currency);
                newMachine.PromoBalance = double.Parse(row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[5]")).Text, System.Globalization.NumberStyles.Currency);
                newMachine.LastPlay = DateTime.Parse(row.FindElement(By.XPath(".//Custom[6]")).Text);

                return newMachine;

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetMachine: Invalid row " + rowNum);
                return null;
            }
        }


        public Machine GetMachine(int machineNumber)
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = machineList.FindElements(RowSelector);

            foreach (var row in rows)
            {
                int machineNo = int.Parse(row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text);

                if (machineNo == machineNumber)
                {
                    Machine newMachine = new Machine();

                    newMachine.MachineNumber = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text;
                    string statusText = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[2]")).Text;
                    newMachine.Status = statusText == "Online";
                    newMachine.Description = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[3]")).Text;
                    newMachine.Balance = double.Parse(row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[4]")).Text, System.Globalization.NumberStyles.Currency);
                    newMachine.PromoBalance = double.Parse(row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[5]")).Text, System.Globalization.NumberStyles.Currency);
                    newMachine.LastPlay = DateTime.Parse(row.FindElement(By.XPath(".//Custom[6]")).Text);

                    return newMachine;
                }
            }

            return null;
        }
    }
}
