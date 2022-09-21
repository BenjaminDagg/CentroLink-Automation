using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace CentroLink_Automation
{
    public class EditPromoScheduleTests : BaseTest
    {

        private LoginPage loginPage;
        private PromoScheduleListPage promoList;
        private AddPromoSchedulePage addPromo;
        private PromoEntrySchedule PromoToDelete;
        private EditPromoSchedulePage editPromo;

        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            loginPage = new LoginPage(driver);
            promoList = new PromoScheduleListPage(driver);
            addPromo = new AddPromoSchedulePage(driver);
            editPromo = new EditPromoSchedulePage(driver);

            PromoToDelete = new PromoEntrySchedule();
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestPromo();

            if (!string.IsNullOrEmpty(PromoToDelete.Description))
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
        public void EditPromo_GoTo_Page()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);
            promoList.ClickEditPromo();
        }


        //Fields are prepopulated with current promo 
        [Test]
        public void EditPromo_GoTo_Prepopulated()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            var currentPromo = promoList.GetPromoSchedule(TestData.TestPromoEntryScheduleId);
            Assert.NotNull(currentPromo);

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);
            promoList.ClickEditPromo();

            string desc = editPromo.GetDescription();

            string startDateString = editPromo.GetPromoStartDate();
            DateTime startDate = DateTime.ParseExact(startDateString, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            string endDateString = editPromo.GetPromoEndDate();
            DateTime endDate = DateTime.ParseExact(endDateString, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            Assert.AreEqual(currentPromo.Description, desc);
            Assert.AreEqual(currentPromo.StartTime, startDate);
            Assert.AreEqual(currentPromo.EndTime, endDate);
        }


        //Start date field is read only if the promo already started
        [Test]
        public async Task EditPromo_PromoStarted_Readonly()
        {

            string startDateString = DateTime.Now.ToString("yyyy-MM-dd HH:00:00.000");
            DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string endDateString = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            await LotteryRetailDatabase.UpdatePromo(TestData.TestPromoEntryScheduleId, startDate, endDate, true, false);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);
            promoList.ClickEditPromo();

            Assert.True(editPromo.IsReadOnly(editPromo.StartDateField));
            Assert.True(editPromo.PromoStarted);
        }


        [Test]
        public async Task EditPromo_PromoEnded_Readonly()
        {

            string startDateString = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string endDateString = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);
            promoList.ClickEditPromo();


            await LotteryRetailDatabase.UpdatePromo(TestData.TestPromoEntryScheduleId, startDate, endDate, true, true);

            Assert.True(editPromo.IsReadOnly(editPromo.EndDateField));
            Assert.True(editPromo.PromoEnded);
        }


        [Test]
        public async Task EditPromo_Success()
        {

            string startDateString = DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string endDateString = DateTime.Now.AddHours(3).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string newStartDate = ParseDateString(startDate);
            string newEndDate = ParseDateString(endDate);
            string newDescription = "New Description";

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            var promoBefore = promoList.GetPromoSchedule(TestData.TestPromoEntryScheduleId);
            Assert.NotNull(promoBefore);

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);
            promoList.ClickEditPromo();

            editPromo.EnterDescription(newDescription);
            editPromo.EnterPromoStartDate(newStartDate);
            editPromo.EnterPromoEndDate(newEndDate);

            editPromo.Save();

            var promoAfter = promoList.GetPromoSchedule(TestData.TestPromoEntryScheduleId);
            Assert.NotNull(promoAfter);

            Assert.False(PromoEntrySchedule.AreEqual(promoAfter, promoBefore));
            Assert.AreEqual(promoAfter.Description,newDescription);
            Assert.AreEqual(promoAfter.StartTime,startDate);
            Assert.AreEqual(promoAfter.EndTime,endDate);
            
        }


        [Test]
        public async Task EditPromo_Cancel()
        {

            string startDateString = DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string endDateString = DateTime.Now.AddHours(3).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string newStartDate = ParseDateString(startDate);
            string newEndDate = ParseDateString(endDate);
            string newDescription = "New Description";

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            var promoBefore = promoList.GetPromoSchedule(TestData.TestPromoEntryScheduleId);
            Assert.NotNull(promoBefore);

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);
            promoList.ClickEditPromo();

            editPromo.EnterDescription(newDescription);
            editPromo.EnterPromoStartDate(newStartDate);
            editPromo.EnterPromoEndDate(newEndDate);

            editPromo.ReturnToPromoList();

            var promoAfter = promoList.GetPromoSchedule(TestData.TestPromoEntryScheduleId);
            Assert.NotNull(promoAfter);

            Assert.True(PromoEntrySchedule.AreEqual(promoAfter, promoBefore));
        }


        //Edit button is disabled if the promo already ended
        [Test]
        public async Task EditPromo_PromoEnded_Edit_Button_Disabled()
        {

            string startDateString = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string endDateString = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            await LotteryRetailDatabase.UpdatePromo(TestData.TestPromoEntryScheduleId, startDate, endDate, true, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);

            Assert.False(driver.FindElement(promoList.EditPromoButton).Enabled);
        }
    }
}
