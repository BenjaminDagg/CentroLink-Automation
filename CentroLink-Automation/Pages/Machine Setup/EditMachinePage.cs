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
    public class EditMachinePage : MachineDetailsPage
    {
        private By RemovedCheckbox;

        protected override By MachineNumberField { get => By.XPath("//Edit[1]"); }
        protected override By LocationMachineNumberField { get => By.XPath("//Edit[2]"); }
        protected override By SerialNumberField { get => By.XPath("//Edit[3]"); }
        protected override By IpAddressField { get => By.XPath("//Edit[4]"); }
        protected override By DescriptionField { get => By.XPath("//Edit[5]"); }

        public EditMachinePage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            RemovedCheckbox = By.XPath("//CheckBox/Text[@ClassName='TextBlock']");
        }



        public void CheckRemoved()
        {
            WindowsElement textBoxRect = (WindowsElement)wait.Until(d => d.FindElement(RemovedCheckbox));
            WindowsElement checkbox = (WindowsElement)driver.FindElement(By.XPath("//CheckBox/Text[@ClassName='TextBlock']/parent::CheckBox"));

            if(checkbox.Selected == false)
            {
                textBoxRect.Click();
            }
        }


        public void UnCheckRemoved()
        {
            WindowsElement textBoxRect = (WindowsElement)wait.Until(d => d.FindElement(RemovedCheckbox));
            WindowsElement checkbox = (WindowsElement)driver.FindElement(By.XPath("//CheckBox/Text[@ClassName='TextBlock']/parent::CheckBox"));

            if (checkbox.Selected == true)
            {
                textBoxRect.Click();
            }
        }


        //Override EnterForm in MachineDetais page. Don't enter Machine Number field because its read-only on edit page
        public override void EnterForm(string machNo, string locationMachNo, string sn, string ipAddress, int bankIndex, int gameIndex)
        {
            EnterLocationMachineNumber(locationMachNo);
            EnterSerialNumber(sn);
            EnterIPAddress(ipAddress);
            SelectBank(bankIndex);
            SelectGame(gameIndex);
        }
    }
}
