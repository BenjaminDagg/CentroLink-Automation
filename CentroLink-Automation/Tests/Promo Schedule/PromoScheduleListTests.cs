using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace CentroLink_Automation
{
    public class PromoScheduleListTests : BaseTest
    {

        private LoginPage loginPage;
        private PromoScheduleListPage promoList;

        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            loginPage = new LoginPage(driver);
            promoList = new PromoScheduleListPage(driver);
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestPromo();
        }


        [Test]
        public void PromoList_GoTo_Page()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();
        }


        [Test]
        public void PromoList_Default_Day_Filter()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            int daysSelected = promoList.GetDayFilter();

            Assert.AreEqual(15, daysSelected);
        }


        [Test]
        public async Task PromoList_Day_Filter()
        {
            //set promo end date back 30 days
            string startTimeString = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd 14:00:00.000");
            DateTime startDate = DateTime.ParseExact(startTimeString, "yyyy-MM-dd 14:00:00.000", CultureInfo.InvariantCulture);

            string endTimeString = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd 15:00:00.000");
            DateTime endDate = DateTime.ParseExact(endTimeString, "yyyy-MM-dd 15:00:00.000", CultureInfo.InvariantCulture);

            await LotteryRetailDatabase.UpdatePromoScheduleDates(startDate, endDate, TestData.TestPromoEntryScheduleId);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            Assert.False(promoList.PromoFoundInList(TestData.TestPromoEntryScheduleId));

            promoList.EnterDayFilter(30);
            promoList.RefreshList();

            Assert.True(promoList.PromoFoundInList(TestData.TestPromoEntryScheduleId));
        }


        //Promos that havent started should appear in the list
        [Test]
        public async Task PromoList_Day_Filter_PromoNotStarted()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            Assert.True(promoList.PromoFoundInList(TestData.TestPromoEntryScheduleId));
        }


        [Test]
        public async Task PromoList_Day_Filter_Future_Promos()
        {
            //set promo for next day
            string startTimeString = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 14:00:00.000");
            DateTime startDate = DateTime.ParseExact(startTimeString, "yyyy-MM-dd 14:00:00.000", CultureInfo.InvariantCulture);

            string endTimeString = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 15:00:00.000");
            DateTime endDate = DateTime.ParseExact(endTimeString, "yyyy-MM-dd 15:00:00.000", CultureInfo.InvariantCulture);

            await LotteryRetailDatabase.UpdatePromoScheduleDates(startDate, endDate, TestData.TestPromoEntryScheduleId);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            Assert.True(promoList.PromoFoundInList(TestData.TestPromoEntryScheduleId));
        }


        //Promos that ended withing the specified number of days should be present in the list
        [Test]
        public async Task PromoList_Day_Filter_EndedWithinDate()
        {
            //set promo for next day
            string startTimeString = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd 14:00:00.000");
            DateTime startDate = DateTime.ParseExact(startTimeString, "yyyy-MM-dd 14:00:00.000", CultureInfo.InvariantCulture);

            string endTimeString = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd 15:00:00.000");
            DateTime endDate = DateTime.ParseExact(endTimeString, "yyyy-MM-dd 15:00:00.000", CultureInfo.InvariantCulture);

            await LotteryRetailDatabase.UpdatePromoScheduleDates(startDate, endDate, TestData.TestPromoEntryScheduleId);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            Assert.True(promoList.PromoFoundInList(TestData.TestPromoEntryScheduleId));

            promoList.EnterDayFilter(14);
            promoList.RefreshList();

            Assert.False(promoList.PromoFoundInList(TestData.TestPromoEntryScheduleId));
        }


        [Test]
        public async Task PromoList_Headers()
        {
            
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            Assert.AreEqual("ID", promoList.GetHeader(0));
            Assert.AreEqual("Description", promoList.GetHeader(1));
            Assert.AreEqual("Start Time", promoList.GetHeader(2));
            Assert.AreEqual("End Time", promoList.GetHeader(3));
            Assert.AreEqual("Started", promoList.GetHeader(4));
            Assert.AreEqual("Ended", promoList.GetHeader(5));
        }


        //Edit button should be disabled if an item isn't selected in the list
        [Test]
        public async Task PromoList_EditButton_Disabled()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            Assert.False(driver.FindElement(promoList.EditPromoButton).Enabled);

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);

            Assert.True(driver.FindElement(promoList.EditPromoButton).Enabled);
        }


        //Delete button should be disabled if an item isn't selected in the list
        [Test]
        public async Task PromoList_DeleteButton_Disabled()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            Assert.False(driver.FindElement(promoList.DeletePromoButton).Enabled);

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);

            Assert.True(driver.FindElement(promoList.DeletePromoButton).Enabled);
        }


        [Test]
        public void PromoList_TogglePromoButton_Text()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.TurnPromoTicketsOn();
            Assert.AreEqual("Turn Promo Ticket Off",driver.FindElement(promoList.TogglePromoButton).Text);

            promoList.TurnPromoTicketsOff();
            Assert.AreEqual("Turn Promo Ticket On", driver.FindElement(promoList.TogglePromoButton).Text);
        }


        [Test]
        public void PromoList_TogglePromo_Confirmation_Dialog()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            string expectedAlert = "Are you sure you want to turn the Promo Ticket Printing";

            promoList.ClickPromoToggleButton();
            string actualAlert = promoList.TogglePromoAlert.AlertText;

            Assert.True(actualAlert.Contains(expectedAlert));
        }


        [Test]
        public void PromoList_TogglePromo_Confirmation_Dialog_Cancel()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            bool promoEnabledBefore = promoList.PromoTicketsAreEnabled;

            promoList.ClickPromoToggleButton();
            promoList.TogglePromoAlert.Cancel();

            bool promoEnabledAfter = promoList.PromoTicketsAreEnabled;

            Assert.AreEqual(promoEnabledAfter,promoEnabledBefore);
        }


        [Test]
        public async Task PromoList_Started_Checkbox()
        {
            string startDateString = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string endDateString = DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            await LotteryRetailDatabase.UpdatePromo(TestData.TestPromoEntryScheduleId, startDate, endDate, true, false);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            var promo = promoList.GetPromoSchedule(TestData.TestPromoEntryScheduleId);

            Assert.True(promo.Started);
            Assert.False(promo.Ended);
        }


        [Test]
        public async Task PromoList_Ended_Checkbox()
        {
            string startDateString = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string endDateString = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            await LotteryRetailDatabase.UpdatePromo(TestData.TestPromoEntryScheduleId, startDate, endDate, true, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            var promo = promoList.GetPromoSchedule(TestData.TestPromoEntryScheduleId);

            Assert.True(promo.Started);
            Assert.True(promo.Ended);
        }
    }
}
