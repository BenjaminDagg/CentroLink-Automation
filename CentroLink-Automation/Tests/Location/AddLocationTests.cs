using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace CentroLink_Automation
{
    public class AddLocationTests : BaseTest
    {

        private LoginPage loginPage;
        private LocationSetupPage locationSetup;
        private AddLocationPage addLocation;
        private EditLocationPage editLocation;
        private Location TestLocation;

        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            TestLocation = await LotteryRetailDatabase.GetLocation(TestData.LocationId);

            loginPage = new LoginPage(driver);
            locationSetup = new LocationSetupPage(driver);
            addLocation = new AddLocationPage(driver);
            editLocation = new EditLocationPage(driver);
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
        public void Test()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var locationCountBefore = locationSetup.RowCount;

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            
        }


        [Test]
        public void AddLocation_Success()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var locationCountBefore = locationSetup.RowCount;

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                TestData.TestLocationDgeId,
                "4065"
            );

            addLocation.Save();
            
            var locationCountAfter = locationSetup.RowCount;

            Assert.Greater(locationCountAfter,locationCountBefore);
        }


        [Test]
        public void AddLocation_DgeId_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var locationCountBefore = locationSetup.RowCount;

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string dgeId = "";
            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                dgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.DgeIdField));
        }


        [Test]
        public void AddLocation_DgeId_Alphanumeric()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var locationCountBefore = locationSetup.RowCount;

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string dgeId = "MOAb1!";
            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                dgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.DgeIdField));
        }


        [Test]
        public void AddLocation_DgeId_Length()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var locationCountBefore = locationSetup.RowCount;

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string dgeId = "MO123";
            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                dgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.DgeIdField));
        }


        //Must be in the format MOXXXX
        [Test]
        [TestCase("123456")]
        [TestCase("1234MO")]
        public void AddLocation_DgeId_Format(string id)
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var locationCountBefore = locationSetup.RowCount;

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string dgeId = id;
            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                dgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.DgeIdField));
        }


        [Test]
        [TestCase("MO0999")]
        [TestCase("MO0001")]

        public void AddLocation_DgeId_Range(string id)
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var locationCountBefore = locationSetup.RowCount;

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string dgeId = id;
            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                dgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.DgeIdField));
        }


        [Test]
        public void AddLocation_DgeId_Space()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var locationCountBefore = locationSetup.RowCount;

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string dgeId = "MO 123";
            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                dgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.DgeIdField));
        }


        [Test]
        public void AddLocation_DgeId_Taken()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var locationCountBefore = locationSetup.RowCount;

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string dgeId = TestData.LocationDgeId;
            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                dgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.DuplicateErrorAlert.IsOpen);
        }


        [Test]
        public void AddLocation_LocationName_Taken()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string locationName = TestData.LocationName;
            addLocation.EnterForm(
                locationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                TestData.TestLocationDgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.DuplicateErrorAlert.IsOpen);
        }


        [Test]
        public void AddLocation_LocationId_Numeric()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string locationId = "123abc";
            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                locationId,
                TestData.TestLocationDgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.LocationIdField));
        }


        [Test]
     
        public void AddLocation_LocationId_Range()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string locationId = TestData.LocationIdMinRange.ToString();
            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                locationId,
                TestData.TestLocationDgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.LocationIdField));

            addLocation.EnterLocationId(TestData.LocationIdMaxRange.ToString());
            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.LocationIdField));
        }


        [Test]
        public void AddLocation_LocationId_Match_DGEID()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string locationId = "1111";
            TestData.TestLocationId = int.Parse(locationId);

            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                locationId,
                TestData.TestLocationDgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.LocationIdField));
        }


        [Test]
        public void AddLocation_LocationId_Taken()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string locationId = TestData.LocationId.ToString();

            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                locationId,
                TestData.TestLocationDgeId,
                "4065"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.DuplicateErrorAlert.IsOpen);
        }



        [Test]
        public void AddLocation_RetailerNumber_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string retailerNumber = "";

            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                TestData.TestLocationDgeId,
                retailerNumber
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.RetailerNumberField));
        }


        [Test]
        [TestCase("123")]
        [TestCase("12345")]
        public void AddLocation_RetailerNumber_Length(string retailNum)
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string retailerNumber = retailNum;

            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                TestData.TestLocationDgeId,
                retailerNumber
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.RetailerNumberField));
        }


        [Test]
        [TestCase("00001")]
        [TestCase("999")]
        public void AddLocation_RetailerNumber_Range(string retailNum)
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            string retailerNumber = retailNum;

            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                TestData.TestLocationDgeId,
                retailerNumber
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.RetailerNumberField));
        }



        [Test]
        public void AddLocation_TPI_NoSelection()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.None,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                TestData.TestLocationDgeId,
                "1234"
            );

            addLocation.ClickSave();

            Assert.True(addLocation.ErrorIsDisplayed(addLocation.TpiDropdown.DropdownButton));
        }


        //Only 1 location should be able to be set as the default location
        [Test]
        public void AddLocation_Multiple_Default()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRowByLocationId(TestData.LocationId);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(false);
            editLocation.Save();

            locationSetup.ClickAddLocation();

            addLocation.EnterForm(
                TestData.TestLocationName,
                "Address1",
                "Address2",
                "City",
                EditLocationPage.TPISetting.DiamondGameBackOffice,
                "CA",
                "12345",
                "6612200748",
                "0",
                "1000",
                "",
                "10000",
                "",
                TestData.TestLocationId.ToString(),
                TestData.TestLocationDgeId,
                "4065"
            );

            addLocation.SetLocationAsDefault(true);
            addLocation.Save();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.SetLocationAsDefault(true);
            editLocation.ClickSave();

            Assert.False(editLocation.SuccessAlert.IsOpen);
        }
    }
}
