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
    }
}
