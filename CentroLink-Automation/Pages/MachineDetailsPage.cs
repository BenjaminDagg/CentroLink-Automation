﻿using NUnit.Framework;
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
    public class MachineDetailsPage : DataGridPage
    {

        protected By IpAddressField;
        protected By MachineNumberField;
        protected By LocationMachineNumberField;
        protected By SerialNumberField;
        protected By DescriptionField;
        protected By GameDropdownSelector;
        public DropdownElement GameDropdown;
        protected By BankDropdownSelector;
        public DropdownElement BankDropdown;
        protected By SaveButton;
        protected By BackButton;
        public ConfirmationWindow ConfirmationWindow { get; set; }
        public ErrorWindow ErrorWindow { get; set; }
        public SuccessWindow SuccessWindow { get; set; } 

        public override By DataGrid { get => new ByAccessibilityId("GameSetupList"); }
        public override By RowSelector { get => By.ClassName("ListViewItem");}

        public MachineDetailsPage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            this.driver = _driver;

            //elements
            MachineNumberField = By.XPath("(//Edit[@IsEnabled='True'])[1]");
            LocationMachineNumberField = By.XPath("(//Edit[@IsEnabled='True'])[2]");
            SerialNumberField = By.XPath("(//Edit[@IsEnabled='True'])[3]");
            IpAddressField = By.XPath("(//Edit[@IsEnabled='True'])[4]");
            DescriptionField = By.XPath("(//Edit[@IsEnabled='True'])[5]");
            SaveButton = By.Name("Save");
            BackButton = By.Name("Back");
            ConfirmationWindow = new ConfirmationWindow(driver);
            ErrorWindow = new ErrorWindow(driver);
            SuccessWindow = new SuccessWindow(driver);

            BankDropdownSelector = By.XPath("(//ComboBox[@ClassName='ComboBox'])[1]");
            BankDropdown = new DropdownElement(BankDropdownSelector, driver);

            GameDropdownSelector = By.XPath("(//ComboBox[@ClassName='ComboBox'])[2]");
            GameDropdown = new DropdownElement(GameDropdownSelector, driver);
        }


        public void EnterMachineNumber(string text)
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(MachineNumberField));

            element.Clear();
            element.SendKeys(text);
        }


        public string GetMachineNumber()
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(MachineNumberField));
            return element.Text;
        }


        public bool MachineNumberErrorIsDisplayed()
        {
            try
            {
                WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(MachineNumberField));
                string helpText = element.GetAttribute("HelpText");

                if (string.IsNullOrEmpty(helpText))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch(Exception ex)
            {
                return false;
            }
        }


        public void EnterLocationMachineNumber(string text)
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(LocationMachineNumberField));

            element.Clear();
            element.SendKeys(text);
        }


        public string GetLocationMachineNumber()
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(LocationMachineNumberField));

            return element.Text;
        }


        public bool LocationMachineNumberErrorIsDisplayed()
        {
            try
            {
                WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(LocationMachineNumberField));
                string helpText = element.GetAttribute("HelpText");

                if (string.IsNullOrEmpty(helpText))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public void EnterSerialNumber(string text)
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(SerialNumberField));

            element.Clear();
            element.SendKeys(text);
        }


        public string GetSerialNumber()
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(SerialNumberField));

            return element.Text;
        }


        public void EnterIPAddress(string text)
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(IpAddressField));

            element.Clear();
            element.SendKeys(text);
        }


        public string GetIPAddress()
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(IpAddressField));

            return element.Text;
        }


        public bool IPAddressErrorIsDisplayed()
        {
            try
            {
                WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(IpAddressField));
                string helpText = element.GetAttribute("HelpText");

                if (string.IsNullOrEmpty(helpText))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public void EnterDescription(string text)
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(DescriptionField));

            element.Clear();
            element.SendKeys(text);
        }


        public string GetDescription()
        {
            WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(DescriptionField));

            return element.Text;
        }


        public bool DescriptionErrorIsDisplayed()
        {
            try
            {
                WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(DescriptionField));
                string helpText = element.GetAttribute("HelpText");

                if (string.IsNullOrEmpty(helpText))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public void ClickBankDropdown()
        {
            BankDropdown.Click();
        }


        public void SelectBank(int index)
        {
            SelectBank(0,index);
        }


        public void SelectBank(string bankDesc)
        {
            SelectBank(0,bankDesc);
        }


        public List<string> GetBankOptions()
        {
            return GetBankOptions(0);
        }


        public string GetSelectedBank()
        {
            return GetSelectedBank(0);
        }


        public void ClickGameDropdown()
        {
            GameDropdown.Click();
        }


        public void SelectGame(int index)
        {
            SelectGame(0,index);
        }


        //If no row number is specified select games from first row in the table.
        public void SelectGame(string gameName)
        {
            SelectGame(0,gameName);
        }


        public List<string> GetGameOptions()
        {
            return GameDropdown.Options;
        }


        public string GetSelectedGame()
        {
            return GameDropdown.SelectedOption;
        }


        public void Save()
        {
            wait.Until(d => d.FindElement(SaveButton)).Click();
        }


        public void ClickBackButton()
        {
            wait.Until(d => d.FindElement(BackButton)).Click();
        }


        public void ReturnToMachineSetup()
        {
            ClickBackButton();
            ConfirmationWindow.Confirm();
        }


        //Select bank for a row in the Game List by index of the bank in the dropdown list.
        public void SelectBank(int rowNum, int optionIndex)
        {
            Console.WriteLine(RowCount);
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By dropdownXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom/ComboBox");
                var bankElement = gameList.FindElement(dropdownXpath);

                DropdownElement bankDropdown = new DropdownElement(dropdownXpath, driver);
                bankDropdown.SelectByIndex(optionIndex);
            }
            catch (Exception ex)
            {

            }
        }


        //Select bank for a row in the Game List by name of the bank in the dropdown list.
        public void SelectBank(int rowNum, string bankName)
        {
            Console.WriteLine(RowCount);
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By dropdownXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom/ComboBox");
                var bankElement = gameList.FindElement(dropdownXpath);

                DropdownElement bankDropdown = new DropdownElement(dropdownXpath, driver);
                bankDropdown.SelectByName(bankName);
            }
            catch (Exception ex)
            {

            }
        }


        public string GetSelectedBank(int rowNum)
        {
            Console.WriteLine(RowCount);
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By dropdownXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom/ComboBox");
                var bankElement = gameList.FindElement(dropdownXpath);

                DropdownElement bankDropdown = new DropdownElement(dropdownXpath, driver);
                return bankDropdown.SelectedOption;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public List<string> GetBankOptions(int rowNum)
        {
            Console.WriteLine(RowCount);
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By dropdownXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom/ComboBox");
                var bankElement = gameList.FindElement(dropdownXpath);

                DropdownElement bankDropdown = new DropdownElement(dropdownXpath, driver);
                return bankDropdown.Options;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public void SelectGame(int rowNum, int optionIndex)
        {
            Console.WriteLine(RowCount);
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By dropdownXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom[2]/ComboBox");
                var gameElement = gameList.FindElement(dropdownXpath);

                DropdownElement gameDropdown = new DropdownElement(dropdownXpath, driver);
                gameDropdown.SelectByIndex(optionIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("MachineDetailsPage.SelectGame: Game dropdown not found for row # " + rowNum);
            }
        }


        public void SelectGame(int rowNum, string gameName)
        {
            Console.WriteLine(RowCount);
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By dropdownXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom[2]/ComboBox");
                var gameElement = gameList.FindElement(dropdownXpath);

                DropdownElement gameDropdown = new DropdownElement(dropdownXpath, driver);
                gameDropdown.SelectByName(gameName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("MachineDetailsPage.SelectGame: Game dropdown not found for row # " + rowNum);
            }
        }


        public string GetSelectedGame(int rowNum)
        {
            Console.WriteLine(RowCount);
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By dropdownXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom[2]/ComboBox");
                var gameElement = gameList.FindElement(dropdownXpath);

                DropdownElement gameDropdown = new DropdownElement(dropdownXpath, driver);
                return gameDropdown.SelectedOption;
            }
            catch (Exception ex)
            {
                Console.WriteLine("MachineDetailsPage.SelectGame: Game dropdown not found for row # " + rowNum);
                return null;
            }
        }


        public List<string> GetGameOptions(int rowNum)
        {
            
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By dropdownXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom[2]/ComboBox");
                var gameElement = gameList.FindElement(dropdownXpath);

                DropdownElement gameDropdown = new DropdownElement(dropdownXpath, driver);
                return gameDropdown.Options;
            }
            catch (Exception ex)
            {
                Console.WriteLine("MachineDetailsPage.SelectGame: Game dropdown not found for row # " + rowNum);
                return null;
            }
        }


        public void EnterForm(string machNo, string locationMachNo, string sn, string ipAddress,int bankIndex, int gameIndex)
        {
            EnterMachineNumber(machNo);
            EnterLocationMachineNumber(locationMachNo);
            EnterSerialNumber(sn);
            EnterIPAddress(ipAddress);
            SelectBank(bankIndex);
            SelectGame(gameIndex);
        }
    }
}
