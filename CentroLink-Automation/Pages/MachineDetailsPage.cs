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
    public class MachineDetailsPage : BasePage
    {

        protected By IpAddressField;
        protected By MachineNumberField;
        protected By LocationMachineNumberField;
        protected By SerialNumberField;
        protected By DescriptionField;
        protected By BankDropdown;
        
        public MachineDetailsPage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            //elements
            MachineNumberField = By.XPath("(//Edit[@IsEnabled='True'])[1]");
            LocationMachineNumberField = By.XPath("(//Edit[@IsEnabled='True'])[2]");
            SerialNumberField = By.XPath("(//Edit[@IsEnabled='True'])[3]");
            IpAddressField = By.XPath("(//Edit[@IsEnabled='True'])[4]");
            DescriptionField = By.XPath("(//Edit[@IsEnabled='True'])[5]");
            BankDropdown = By.XPath("(//ComboBox[@ClassName='ComboBox'])[1]");
        }


        public void EnterMachineNumber(string text)
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(MachineNumberField));

            element.Clear();
            element.SendKeys(text);
        }


        public void EnterLocationMachineNumber(string text)
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(LocationMachineNumberField));

            element.Clear();
            element.SendKeys(text);
        }


        public void EnterSerialNumber(string text)
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(SerialNumberField));

            element.Clear();
            element.SendKeys(text);
        }


        public void EnterIPAddress(string text)
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(IpAddressField));

            element.Clear();
            element.SendKeys(text);
        }


        public void EnterDescription(string text)
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(DescriptionField));

            element.Clear();
            element.SendKeys(text);
        }


        public void ClickBankDropdown()
        {
            WindowsElement dropDownBtn = (WindowsElement)wait.Until(d => d.FindElement(BankDropdown));
            dropDownBtn.Click();
        }


        public void SelectBank(int index)
        {
            ClickBankDropdown();

            WindowsElement dropdownList = (WindowsElement)wait.Until(d => d.FindElement(By.ClassName("Popup")));
            var options = dropdownList.FindElements(By.ClassName("ListBoxItem"));

            try
            {
                options[index].Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Select Bank: Option at index {index} not fouind");
            }
        }


        public void SelectBank(string bankDesc)
        {
            WindowsElement dropDownBtn = (WindowsElement)wait.Until(d => d.FindElement(BankDropdown));
            dropDownBtn.Click();

            WindowsElement dropdownList = (WindowsElement)wait.Until(d => d.FindElement(By.ClassName("Popup")));
            var options = dropdownList.FindElements(By.ClassName("ListBoxItem"));

            foreach(var option in options)
            {
                string bank = option.FindElement(By.XPath("//Text")).Text;


                if (bank == bankDesc)
                {
                    option.Click();
                    return;
                }
            }

            dropDownBtn.Click();
        }


        public List<string> GetBankOptions()
        {
            WindowsElement dropDownBtn = (WindowsElement)wait.Until(d => d.FindElement(BankDropdown));
            dropDownBtn.Click();

            WindowsElement dropdownList = (WindowsElement)wait.Until(d => d.FindElement(By.ClassName("Popup")));
            var options = dropdownList.FindElements(By.ClassName("ListBoxItem"));

            List<string> bankOptions = new List<string>();

            foreach(var option in options)
            {
                var text = option.FindElement(By.XPath("//Text")).Text;
                bankOptions.Add(text);
            }

            //Click away from the dropdown to close it
            dropDownBtn.Click();

            return bankOptions;
        }


        public string GetSelectedBank()
        {
            //SelectionItem.IsSelected
            Thread.Sleep(1000);
            ClickBankDropdown();

            WindowsElement dropdownList = (WindowsElement)wait.Until(d => d.FindElement(By.ClassName("Popup")));
            var options = dropdownList.FindElements(By.ClassName("ListBoxItem"));

            try
            {
                foreach(var opt in options)
                {
                    
                    Console.WriteLine(opt.GetAttribute("HasKeyboardFocus"));
                    if (opt.GetAttribute("HasKeyboardFocus") == "True")
                    {
                        string text = opt.FindElement(By.XPath("//Text")).Text;
                        return text;
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}
