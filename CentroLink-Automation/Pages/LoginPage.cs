using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;   //DesiredCapabilities
using System;
using Appium;
using OpenQA.Selenium.Appium;   //Appium Options
using System.Threading;
using OpenQA.Selenium;

namespace CentroLink_Automation
{
    public class LoginPage : BasePage
    {

        private ByAccessibilityId UserNameField;
        private By PasswordField;
        private ByAccessibilityId LoginButton;

        public LoginPage(WindowsDriver<WindowsElement> _driver) : base(_driver)
        {
            driver = _driver;

            UserNameField = new ByAccessibilityId("UserName");
            PasswordField = By.XPath("//Edit[2]");
            LoginButton = new ByAccessibilityId("Login");
        }


        public void EnterUserName(string username)
        {
            wait.Until(d => d.FindElement(UserNameField));
            driver.FindElement(UserNameField).SendKeys(username);
        }


        public void EnterPassword(string password)
        {
            wait.Until(d => d.FindElement(PasswordField));
            driver.FindElement(PasswordField).SendKeys(password);
        }


        public void ClickLoginButton()
        {
            wait.Until(d => d.FindElement(LoginButton));
            driver.FindElement(LoginButton).Click();
        }


        public void Login(string username, string password)
        {
            EnterUserName(username);
            EnterPassword(password);
            ClickLoginButton();
        }
    }
}
