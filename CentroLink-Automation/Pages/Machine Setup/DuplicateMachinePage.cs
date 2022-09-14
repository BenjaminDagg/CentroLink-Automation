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

namespace CentroLink_Automation
{
    public class DuplicateMachinePage : MachineDetailsPage
    {
        private By MachineNumberToDuplicateField;
        private By LocationMachineNumberToDuplicateField;
        protected override By MachineNumberField { get => By.XPath("//Edit[1]"); }
        protected override By LocationMachineNumberField { get => By.XPath("//Edit[3]"); }
        protected override By SerialNumberField { get => By.XPath("//Edit[5]"); }
        protected override By IpAddressField { get => By.XPath("//Edit[6]"); }
        protected override By DescriptionField { get => By.XPath("//Edit[7]"); }

        public DuplicateMachinePage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            MachineNumberToDuplicateField = By.XPath("//Edit[2]");
            LocationMachineNumberToDuplicateField = By.XPath("//Edit[4]");
        }


        public string GetMachineNumberToDuplicate()
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(MachineNumberToDuplicateField));

            return element.Text;
        }


        public string GetLocationMachineNumberToDuplicate()
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(LocationMachineNumberToDuplicateField));

            return element.Text;
        }


        public override bool IsReadOnly(MachineFields field)
        {
            By target;

            switch (field)
            {
                case MachineFields.MachineNumber:
                    target = MachineNumberField;
                    break;
                case MachineFields.LocationMachineNumber:
                    target = LocationMachineNumberField;
                    break;
                case MachineFields.SerialNumber:
                    target = SerialNumberField;
                    break;
                case MachineFields.IPAddress:
                    target = IpAddressField;
                    break;
                case MachineFields.Description:
                    target = DescriptionField;
                    break;
                case MachineFields.MachineNumberToDuplicate:
                    target = MachineNumberToDuplicateField;
                    break;
                case MachineFields.LocationMachineNumberToDuplicate:
                    target = LocationMachineNumberToDuplicateField;
                    break;
                default:
                    target = MachineNumberField;
                    break;
            }

            try
            {
                driver.FindElement(target).Clear();
                driver.FindElement(target).SendKeys("test");
            }
            catch (Exception ex)
            {
                return true;
            }

            return false;
        }
    }
}
