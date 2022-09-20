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
        private Location TestLocation;

        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            TestLocation = await LotteryRetailDatabase.GetLocation(TestData.LocationId);

            loginPage = new LoginPage(driver);
            locationSetup = new LocationSetupPage(driver);
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
        public void EditLocation_Success()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            var locationBefore = locationSetup.GetLocation();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            string newLocationName = "American Eagle3";
            editLocation.EnterLocationName(newLocationName);
            editLocation.Save();

            var locationAfter = locationSetup.GetLocation();

            Assert.AreNotEqual(locationBefore.LocationName,locationAfter.LocationName);
            Assert.AreEqual(newLocationName, locationAfter.LocationName);
        }


        [Test]
        public void EditLocation_DgeId_ReadOnly()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            Assert.True(editLocation.IsReadOnly(editLocation.DgeIdField));
        }


        [Test]
        public void EditLocation_LocationName_Missing()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            Assert.False(editLocation.ErrorIsDisplayed(editLocation.LocationNameField));

            editLocation.EnterLocationName("");
            editLocation.ClickSave();

            Assert.True(editLocation.ErrorIsDisplayed(editLocation.LocationNameField));
        }


        //Location Name field allows alphanumeric characters
        [Test]
        public void EditLocation_LocationName_Alphanumeric()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            Assert.False(editLocation.ErrorIsDisplayed(editLocation.LocationNameField));

            editLocation.EnterLocationName("abcABC123!@#");
            editLocation.ClickSave();

            Assert.False(editLocation.ErrorIsDisplayed(editLocation.LocationNameField));
        }


        [Test]
        public void EditLocation_LocationName_MaxLength()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            string newLocationName = new string('a',TestData.LocationNameMaxCharacters + 1);
            editLocation.EnterLocationName(newLocationName);
            editLocation.ClickSave();

            Assert.False(editLocation.ErrorIsDisplayed(editLocation.LocationNameField));

            editLocation.SuccessAlert.Confirm();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            string locationNameAfter = editLocation.GetLocationName();
            Assert.LessOrEqual(locationNameAfter.Length, TestData.LocationNameMaxCharacters);
        }


        [Test]
        public void EditLocation_LocationId_ReadOnly()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            Assert.True(editLocation.IsReadOnly(editLocation.LocationIdField));
        }


        [Test]
        public void EditLocation_RetailerNumber_ReadOnly()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            Assert.True(editLocation.IsReadOnly(editLocation.RetailerNumberField));
        }


        [Test]
        public void EditLocation_SweepAmount_InvalidLength()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            string newSweepAccount = "12345";
            editLocation.EnterSweepAmount(newSweepAccount);

            editLocation.ClickSave();
            Assert.True(editLocation.ErrorIsDisplayed(editLocation.SweepAmountField));
        }


        [Test]
        public void EditLocation_SweepAmount_CorrectLength()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            string newSweepAccount = new string('1', 16);
            editLocation.EnterSweepAmount(newSweepAccount);

            editLocation.ClickSave();
            Assert.False(editLocation.ErrorIsDisplayed(editLocation.SweepAmountField));
        }


        [Test]
        public void EditLocation_SweepAmount_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterSweepAmount("");

            editLocation.ClickSave();
            Assert.False(editLocation.ErrorIsDisplayed(editLocation.SweepAmountField));
        }


        [Test]
        public void EditLocation_PostCode_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterPostalCode("");

            editLocation.ClickSave();
            Assert.True(editLocation.ErrorIsDisplayed(editLocation.PostalCodeField));
        }


        [Test]
        [TestCase("1234")]
        [TestCase("123456")]
        public void EditLocation_PostCode_InvalidLength(string postCode)
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterPostalCode(postCode);

            editLocation.ClickSave();
            Assert.True(editLocation.ErrorIsDisplayed(editLocation.PostalCodeField));
        }


        [Test]
        public void EditLocation_PayoutAuthorization_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterPayoutAuthorizationAmount("");
            editLocation.ClickBackButton();

            Assert.True(editLocation.ErrorIsDisplayed(editLocation.PayoutAuthorizationAmount));
        }


        [Test]
        [TestCase(0.0)]
        [TestCase(214000.01)]
        public void EditLocation_PayoutAuthorization_InvalidAmount(double amount)
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterPayoutAuthorizationAmount(amount.ToString());
            editLocation.ClickSave();

            Assert.True(editLocation.ErrorIsDisplayed(editLocation.PayoutAuthorizationAmount));
        }


        [Test]
        public void EditLocation_MaxBalanceAdjustment_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterMaxBetAdjustment("");
            editLocation.ClickBackButton();

            Assert.True(editLocation.ErrorIsDisplayed(editLocation.MaxBalanceAdjustmentField));
        }


        [Test]
        [TestCase(0.0)]
        [TestCase(2500.01)]
        public void EditLocation_MaxBalanceAdjustment_InvalidAmount(double amount)
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterMaxBetAdjustment(amount.ToString());
            editLocation.ClickSave();

            Assert.True(editLocation.ErrorIsDisplayed(editLocation.MaxBalanceAdjustmentField));
        }


        [Test]
        public void EditLocation_TPI_Default_Selection()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            string selected = editLocation.TpiDropdown.SelectedOption;
            Assert.AreEqual("Diamond Game Backoffice",selected);
        }


        [Test]
        public void EditLocation_CashTimeout_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterCashoutTimeout("");
            editLocation.ClickBackButton();

            Assert.True(editLocation.ErrorIsDisplayed(editLocation.CashoutTimeoutField));
        }


        [Test]
        public void EditLocation_CashTimeout_Zero()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterCashoutTimeout("0");
            editLocation.ClickSave();

            Assert.True(editLocation.ErrorIsDisplayed(editLocation.CashoutTimeoutField));
        }


        [Test]
        public void EditLocation_Address_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterAddress1("");
            editLocation.ClickSave();

            Assert.True(editLocation.ErrorIsDisplayed(editLocation.Address1Field));
        }


        [Test]
        public void EditLocation_City_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterCity("");
            editLocation.ClickSave();

            Assert.True(editLocation.ErrorIsDisplayed(editLocation.CityField));
        }


        [Test]
        public void EditLocation_State_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickLocationSetupTab();

            locationSetup.SelectRow(0);
            locationSetup.ClickEditLocation();

            editLocation.EnterState("");
            editLocation.ClickSave();

            Assert.True(editLocation.ErrorIsDisplayed(editLocation.StateField));
        }
    }
}
