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
    public class MachineSetupPage : DataGridPage
    {
        By AddMachineButton;
        By EditMachineButton;
        By DuplicateMachineButton;
        By RefreshButton;
        By ShowRemovedCheckbox;

        public MachineSetupPage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            //elements
            AddMachineButton = By.Name("Add New Machine");
            EditMachineButton = By.Name("Edit Selected Machine");
            DuplicateMachineButton = By.Name("Duplicate Selected Machine");
            RefreshButton = By.Name("Refresh");
            ShowRemovedCheckbox = By.Name("Show Removed Machines");
        }


        public bool IsShowingRemoved
        {
            get
            {
                WindowsElement checkbox = (WindowsElement)wait.Until(d => d.FindElement(ShowRemovedCheckbox));

                return checkbox.Selected;
            }
        }


        public void ClickAddMachine()
        {
            WindowsElement addMachineBtn = (WindowsElement)wait.Until(d => d.FindElement(AddMachineButton));
            addMachineBtn.Click();
        }


        public void ClickEditMachine()
        {
            WindowsElement editMachineBtn = (WindowsElement)wait.Until(d => d.FindElement(EditMachineButton));
            editMachineBtn.Click();
        }


        public void ClickDuplicateMachine()
        {
            WindowsElement dupMachineBtn = (WindowsElement)wait.Until(d => d.FindElement(DuplicateMachineButton));
            dupMachineBtn.Click();
        }


        public void ClickRefreshButton()
        {
            WindowsElement refreshBtn = (WindowsElement)wait.Until(d => d.FindElement(RefreshButton));
            refreshBtn.Click();
        }


        public void DisplayMachineList()
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = machineList.FindElements(By.ClassName("DataGridRow"));

            foreach (var row in rows)
            {
                string machineNo = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text;
                Console.WriteLine(machineNo);
            }
        }


        public void ShowRemovedMachines()
        {
            if (IsShowingRemoved == false)
            {
                WindowsElement checkbox = (WindowsElement)wait.Until(d => d.FindElement(ShowRemovedCheckbox));
                checkbox.Click();
            }
        }


        public void HideRemovedMachines()
        {
            if (IsShowingRemoved == true)
            {
                WindowsElement checkbox = (WindowsElement)wait.Until(d => d.FindElement(ShowRemovedCheckbox));
                checkbox.Click();
            }
        }


        public bool MachineFoundInList(string machineNumber)
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = machineList.FindElements(By.ClassName("DataGridRow"));

            foreach (var row in rows)
            {
                string machineNo = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text;
                Console.WriteLine(machineNo);
                if(machineNo == machineNumber)
                {
                    return true;
                }
            }

            return false;
        }


        public int RowNumberOfMachine(string machineNumber)
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = machineList.FindElements(By.ClassName("DataGridRow"));

            int i = 0;
            foreach (var row in rows)
            {
                string machineNo = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text;
                
                if (machineNo == machineNumber)
                {
                    return i;
                }
                i++;
            }

            return -1;
        }


        public bool MachineIsActive(string machineNumber)
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = machineList.FindElements(By.ClassName("DataGridRow"));

            foreach (var row in rows)
            {
                string machineNo = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text;
                
                if (machineNo == machineNumber)
                {
                    string status = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[4]")).Text;
     
                    return status == "Active";
                }
            }

            return false;
        }


        public bool MachineIsRemoved(string machineNumber)
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = machineList.FindElements(By.ClassName("DataGridRow"));

            foreach (var row in rows)
            {
                string machineNo = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text;

                if (machineNo == machineNumber)
                {
                    string status = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[6]")).Text;

                    return status == "Yes";
                }
            }

            return false;
        }


        public Machine GetMachineAtRow(int rowNum)
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = machineList.FindElements(By.ClassName("DataGridRow"));

            try
            {
                var row = machineList.FindElement(By.XPath("(.//DataItem[@ClassName='DataGridRow'])[" + (rowNum + 1) + "]"));

                Machine newMachine = new Machine();

                newMachine.MachineNumber = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text;
                newMachine.LocationMachineNumber = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[2]")).Text;
                newMachine.Description = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[3]")).Text;
                
                string statusText = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[4]")).Text;
                newMachine.Status = statusText == "Active";

                newMachine.IPAddress = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[5]")).Text;

                string removedText = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[6]")).Text;
                newMachine.Removed = removedText == "Yes";

                newMachine.SerialNumber = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[7]")).Text;
                newMachine.OperatingSystemVersion = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[8]")).Text;

                return newMachine;

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetMachine: Invalid row " + rowNum);
                return null;
            }
        }


        public void SelectRowByMachineNumber(string machineNumber)
        {
            WindowsElement machineList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));
            var rows = machineList.FindElements(By.ClassName("DataGridRow"));

            foreach (var row in rows)
            {
                string machineNo = row.FindElement(By.XPath("(//Custom[@ClassName='DataGridCell'])[1]")).Text;

                if (machineNo == machineNumber)
                {
                    row.Click();
                    return;
                }
            }
        }
    }
}
