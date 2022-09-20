using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace CentroLink_Automation
{
    public class EditLocationTests : BaseTest
    {

        private LoginPage loginPage;
        private LocationSetupPage locationSetup;
        private EditLocationPage editLocation;

        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            loginPage = new LoginPage(driver);
            locationSetup = new LocationSetupPage(driver);
            editLocation = new EditLocationPage(driver);
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();
        }


        [Test]
        public void Test()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            
            editLocation.SelectTPI(EditLocationPage.TPISetting.DiamondGameBackOffice);
            Thread.Sleep(3000);
            editLocation.SelectTPI(EditLocationPage.TPISetting.SierraDesignGroup);
            Thread.Sleep(3000);
            editLocation.SelectTPI(EditLocationPage.TPISetting.MultiMediaGamesInc);
            Thread.Sleep(3000);
            editLocation.SelectTPI(EditLocationPage.TPISetting.IowaStateLottery);
            Thread.Sleep(3000);
            editLocation.SelectTPI(EditLocationPage.TPISetting.SlotAccountingSystem);
            Thread.Sleep(3000);

        }
    }
}
