using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace CentroLink_Automation
{

    public class LocationSetupTests : BaseTest
    {

        private LoginPage loginPage;
        private LocationSetupPage locationSetup;
        private Location TestLocation;
        private EditLocationPage editLocation;

        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            loginPage = new LoginPage(driver);
            locationSetup = new LocationSetupPage(driver);
            editLocation = new EditLocationPage(driver);

            TestLocation = await LotteryRetailDatabase.GetLocation(TestData.LocationId);
        }

        
        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetLocation(
                TestLocation.DgeId,
                TestLocation.LocationId,
                TestLocation.LocationName,
                TestLocation.RetailerNumber,
                true,
                TestLocation.AccountDayStart,
                TestLocation.AccountDayEnd,
                TestLocation.SweepAmount
            );
        }


        [Test]
        public void LocationSetup_GoTo_Page()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();
           
        }


        [Test]
        public void LocationSetup_Headers()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();
            
            Assert.AreEqual("Dge Id", locationSetup.GetHeader(0));
            Assert.AreEqual("Location Id", locationSetup.GetHeader(1));
            Assert.AreEqual("Location Name", locationSetup.GetHeader(2));
            Assert.AreEqual("Retailer #", locationSetup.GetHeader(3));
            Assert.AreEqual("Is Default", locationSetup.GetHeader(4));
            Assert.AreEqual("Account Day Start", locationSetup.GetHeader(5));
            Assert.AreEqual("Account Day End", locationSetup.GetHeader(6));
        }


        [Test]
        public void LocationSetup_AddButton_Disabled()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var location = locationSetup.GetLocation();
            Assert.True(location.IsDefault);

            Assert.False(driver.FindElement(locationSetup.AddLocationButton).Enabled);
        }


        //Edit button is disabled until user selects a location from the list
        [Test]
        public void LocationSetup_EditButton_Disabled()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            Assert.False(driver.FindElement(locationSetup.EditLocationButton).Enabled);

            locationSetup.SelectRow(0);

            Assert.True(driver.FindElement(locationSetup.EditLocationButton).Enabled);
        }


        [Test]
        public async Task LocationSetup_Data()
        {
            var expectedLocation = await LotteryRetailDatabase.GetLocation(TestData.LocationId);
            string startDayString = expectedLocation.AccountDayStart.ToString("hh:mm:ss");
            expectedLocation.AccountDayStart = DateTime.ParseExact(startDayString, "hh:mm:ss", CultureInfo.InvariantCulture);
            string endDayString = expectedLocation.AccountDayEnd.ToString("hh:mm:ss");
            expectedLocation.AccountDayEnd = DateTime.ParseExact(endDayString, "hh:mm:ss", CultureInfo.InvariantCulture);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var location = locationSetup.GetLocation();

            Assert.AreEqual(expectedLocation.DgeId,location.DgeId);
            Assert.AreEqual(expectedLocation.LocationId, location.LocationId);
            Assert.AreEqual(expectedLocation.LocationName, location.LocationName);
            Assert.AreEqual(expectedLocation.RetailerNumber, location.RetailerNumber);
            Assert.AreEqual(expectedLocation.IsDefault, location.IsDefault);
            Assert.AreEqual(expectedLocation.AccountDayStart, location.AccountDayStart);
            Assert.AreEqual(expectedLocation.AccountDayEnd, location.AccountDayEnd);
        }


        [Test]
        public async Task LocationSetup_Refresh()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var locationBefore = locationSetup.GetLocation();

            //update location in database
            await LotteryRetailDatabase.ResetLocation(
                TestLocation.DgeId,
                TestLocation.LocationId,
                "American Eagle3",
                TestLocation.RetailerNumber,
                true,
                TestLocation.AccountDayStart,
                TestLocation.AccountDayEnd,
                TestLocation.SweepAmount
            );

            locationSetup.Refresh();

            var locationAfter = locationSetup.GetLocation();

            Assert.AreEqual("American Eagle3",locationAfter.LocationName);
            Assert.AreNotEqual(locationBefore.LocationName,locationAfter.LocationName);
        }


        [Test]
        public async Task LocationSetup_Enable_Add_Button()
        {
            
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            Assert.True(driver.FindElement(locationSetup.AddLocationButton).Enabled);
           
        }

    }
}
