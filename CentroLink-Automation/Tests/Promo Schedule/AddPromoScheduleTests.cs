using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace CentroLink_Automation
{
    public class AddPromoScheduleTests : BaseTest
    {

        private LoginPage loginPage;
        private PromoScheduleListPage promoList;
        private AddPromoSchedulePage addPromo;
        private PromoEntrySchedule PromoToDelete;


        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            loginPage = new LoginPage(driver);
            promoList = new PromoScheduleListPage(driver);
            addPromo = new AddPromoSchedulePage(driver);

            PromoToDelete = new PromoEntrySchedule();
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestPromo();

            if(!string.IsNullOrEmpty(PromoToDelete.Description))
            {
                await LotteryRetailDatabase.DeletePromo(PromoToDelete.Description);
            }

            PromoToDelete = null;
        }


        private string ParseDateString(DateTime date)
        {
            var dateString = date.ToString("MM/dd/yyyy hh:00 tt");
            Console.WriteLine("got string: " + dateString);
            if (dateString.IndexOf("AM") != -1)
            {
                dateString = dateString.Replace("AM", "am");
            }
            else
            {
                dateString = dateString.Replace("PM", "pm");
            }

            return dateString;
        }


        [Test]
        public void AddPromo_GoTo_Page()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();
        }


        [Test]
        public void AddPromo_Description_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();

            string startDate = ParseDateString(DateTime.Now.AddHours(1));
            string endDate = ParseDateString(DateTime.Now.AddHours(2));

            addPromo.EnterPromoStartDate(startDate);
            addPromo.EnterPromoEndDate(endDate);
            
            addPromo.ClickSave();

            Assert.True(addPromo.ErrorIsDisplayed(addPromo.Description));
        }


        [Test]
        public void AddPromo_Description_MaxLength()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();

            string startDate = ParseDateString(DateTime.Now.AddHours(1));
            string endDate = ParseDateString(DateTime.Now.AddHours(2));

            string description = new string('a', 129);

            addPromo.EnterDescription(description);
            addPromo.EnterPromoStartDate(startDate);
            addPromo.EnterPromoEndDate(endDate);

            addPromo.ClickSave();

            Assert.True(addPromo.SuccessAlert.IsOpen);

            string descriptionAfter = addPromo.GetDescription();
            Assert.AreEqual(128,description.Length);
        }


        [Test]
        public void AddPromo_StartDate_Past()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();

            string startDate = ParseDateString(DateTime.Now.AddHours(-1));
            string endDate = ParseDateString(DateTime.Now.AddHours(1));
            string description = "Test description";

            addPromo.EnterDescription(description);
            addPromo.EnterPromoStartDate(startDate);
            addPromo.EnterPromoEndDate(endDate);

            addPromo.ClickSave();

            Assert.False(addPromo.SuccessAlert.IsOpen);
        }


        [Test]
        public void AddPromo_StartDate_AfterEnd()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();

            string startDate = ParseDateString(DateTime.Now.AddHours(2));
            string endDate = ParseDateString(DateTime.Now.AddHours(1));
            string description = "Test description";

            addPromo.EnterDescription(description);
            addPromo.EnterPromoStartDate(startDate);
            addPromo.EnterPromoEndDate(endDate);

            addPromo.ClickSave();

            Assert.False(addPromo.SuccessAlert.IsOpen);
        }


        [Test]
        public void AddPromo_EndDate_GreaterThan_1Year()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();

            string startDate = ParseDateString(DateTime.Now.AddHours(1));
            string endDate = ParseDateString(DateTime.Now.AddDays(366));
            string description = "Test description";

            addPromo.EnterDescription(description);
            addPromo.EnterPromoStartDate(startDate);
            addPromo.EnterPromoEndDate(endDate);

            addPromo.ClickSave();

            Assert.False(addPromo.SuccessAlert.IsOpen);
        }


        [Test]
        public void AddPromo_LessThan_1Hour()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();

            string startDate = ParseDateString(DateTime.Now.AddHours(1));
            string endDate = ParseDateString(DateTime.Now.AddMinutes(90));
            string description = "Test description";

            addPromo.EnterDescription(description);
            addPromo.EnterPromoStartDate(startDate);
            addPromo.EnterPromoEndDate(endDate);

            addPromo.ClickSave();

            Assert.False(addPromo.SuccessAlert.IsOpen);
        }


        [Test]
        public void AddPromo_Success()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();

            string description = "Test Promo";
            string startDate = ParseDateString(DateTime.Now.AddHours(1));
            string endDate = ParseDateString(DateTime.Now.AddHours(2));

            PromoToDelete = new PromoEntrySchedule{ Description = description};

            addPromo.EnterDescription(description);
            addPromo.EnterPromoStartDate(startDate);
            addPromo.EnterPromoEndDate(endDate);

            addPromo.Save();

            Assert.True(promoList.PromoFoundInList(description));
        }


        [Test]
        public void AddPromo_Cancel()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();

            string description = "Test Promo";
            string startDate = ParseDateString(DateTime.Now.AddHours(1));
            string endDate = ParseDateString(DateTime.Now.AddHours(2));

            addPromo.EnterDescription(description);
            addPromo.EnterPromoStartDate(startDate);
            addPromo.EnterPromoEndDate(endDate);

            addPromo.ReturnToPromoList();

            Assert.False(promoList.PromoFoundInList(description));
        }
    }
}
