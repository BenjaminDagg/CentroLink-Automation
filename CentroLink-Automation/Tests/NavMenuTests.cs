using NUnit.Framework;
using System.Threading;
using System;

namespace CentroLink_Automation
{
    public class NavMenuTests : BaseTest
    {
        private LoginPage loginPage;

        [SetUp]
        public void Setup()
        {
            loginPage = new LoginPage(driver);
        }

        [Test]
        public void ClickMaintenanceTab()
        {
            loginPage.Login("user1", "Diamond1#");

            navMenu.ExpandAll();
            Assert.Pass();
        }


        [Test]
        public void ClickMachineSetup()
        {
            loginPage.Login("user1", "Diamond1#");

            navMenu.ClickMachineSetupTab();
            
            Assert.Pass();
        }
    }
}
