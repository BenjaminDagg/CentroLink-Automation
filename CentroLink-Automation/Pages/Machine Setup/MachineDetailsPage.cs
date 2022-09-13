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
    public class MachineDetailsPage : DataGridPage
    {

        protected virtual By IpAddressField { get; set; }
        protected virtual By MachineNumberField { get; set; }
        protected virtual By LocationMachineNumberField { get; set; }
        protected virtual By SerialNumberField { get; set; }
        protected virtual By DescriptionField { get; set; }
        protected virtual By SaveButton { get; set; }
        protected virtual By BackButton { get; set; }
        protected By AddGameButton;
        public MultiChoiceAlertWindow ConfirmationWindow { get; set; }
        public SingleChoiceAlertWindow ErrorWindow { get; set; }
        public SingleChoiceAlertWindow SuccessWindow { get; set; } 
        public MultiChoiceAlertWindow RemoveGameWindow { get; set; }
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
            AddGameButton = new ByAccessibilityId("AddGame");
            SaveButton = By.Name("Save");
            BackButton = By.Name("Back");
            ConfirmationWindow = new MultiChoiceAlertWindow(driver,By.Name("Confirm Action"));
            ErrorWindow = new SingleChoiceAlertWindow(driver,By.Name("Error"));
            SuccessWindow = new SingleChoiceAlertWindow(driver, By.Name("Success"));
            RemoveGameWindow = new MultiChoiceAlertWindow(driver,By.Name("Remove Game?"));
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


        public bool SerialNumberErrorIsDisplayed()
        {
            try
            {
                WindowsElement element = (WindowsElement)wait.Until(d => d.FindElement(SerialNumberField));
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


        public bool BankErrorIsDisplayed()
        {
            return BankErrorIsDisplayedForGameNum(0);
        }


        public bool BankErrorIsDisplayedForGameNum(int rowNum)
        {
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By dropdownXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom/ComboBox");
                var bankElement = gameList.FindElement(dropdownXpath);

                string helpText = bankElement.GetAttribute("HelpText");

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
            return GetGameOptions(0);
        }


        public string GetSelectedGame()
        {
            return GetSelectedGame(0);
        }


        public bool GameErrorIsDisplayed()
        {
            return GameErrorIsDisplayedForRow(0);
        }


        public bool GameErrorIsDisplayedForRow(int rowNum)
        {
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By dropdownXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom[2]/ComboBox");
                var gameElement = gameList.FindElement(dropdownXpath);

                string helpText = gameElement.GetAttribute("HelpText");

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
                Console.WriteLine("MachineDetailsPage.SelectGame: Game dropdown not found for row # " + rowNum);
                return false;
            }
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


        public virtual void EnterForm(string machNo, string locationMachNo, string sn, string ipAddress,int bankIndex, int gameIndex)
        {
            EnterMachineNumber(machNo);
            EnterLocationMachineNumber(locationMachNo);
            EnterSerialNumber(sn);
            EnterIPAddress(ipAddress);
            SelectBank(bankIndex);
            SelectGame(gameIndex);
        }


        public bool GameIsEnabled(int rowNum)
        {
            Console.WriteLine(RowCount);
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By checkboxXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom[3]/CheckBox");
                var checkbox = gameList.FindElement(checkboxXpath);

                return checkbox.Selected;
            }
            catch (Exception ex)
            {
                Console.WriteLine("MachineDetailsPage.SelectGame: Game dropdown not found for row # " + rowNum);
                return false;
            }
        }


        public void SetGameEnabledByRow(int rowNum,bool isEnabled)
        {
            Console.WriteLine(RowCount);
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By checkboxXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom[3]/CheckBox");
                var checkbox = gameList.FindElement(checkboxXpath);

                if (checkbox.Selected)
                {
                    if(isEnabled == false)
                    {
                        checkbox.Click();
                    }
                }
                else
                {
                    if(isEnabled == true)
                    {
                        checkbox.Click();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MachineDetailsPage.SelectGame: Game dropdown not found for row # " + rowNum);
                
            }
        }


        private void ClickRemoveGame(int rowNum)
        {
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By removeBtnXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom[4]/Button");
                var button = gameList.FindElement(removeBtnXpath);

                button.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine("MachineDetailsPage.SelectGame: Game dropdown not found for row # " + rowNum);
                
            }
        }


        public void RemoveGameByRow(int rowNum)
        {
            WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

            try
            {
                By removeBtnXpath = By.XPath(".//DataItem[" + (rowNum + 1) + "]/Custom[4]/Button");
                var button = gameList.FindElement(removeBtnXpath);

                button.Click();
                RemoveGameWindow.Confirm();
            }
            catch (Exception ex)
            {
                Console.WriteLine("MachineDetailsPage.SelectGame: Game dropdown not found for row # " + rowNum);

            }
        }


        public bool AddGameIsEnabled()
        {
            try
            {
                WindowsElement addGameBtn = (WindowsElement)wait.Until(d => d.FindElement(AddGameButton));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool RemoveGameIsEnabled()
        {
            try
            {
                WindowsElement gameList = (WindowsElement)wait.Until(d => d.FindElement(DataGrid));

                By removeBtnXpath = By.XPath(".//DataItem[1]/Custom[4]/Button");
                var button = gameList.FindElement(removeBtnXpath);

                return button.Enabled;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("MachineDetailsPage.SelectGame: Game dropdown not found for row # ");
                return false;
            }
        }


        public void ClickAddGame()
        {
            if(AddGameIsEnabled() == true)
            {
                int gameCount = RowCount;

                WindowsElement addGameBtn = (WindowsElement)wait.Until(d => d.FindElement(AddGameButton));
                addGameBtn.Click();

                //wait for new row to appear in the list
                wait.Until(d => d.FindElements(RowSelector).Count == (gameCount + 1));
            }
        }
    }
}
