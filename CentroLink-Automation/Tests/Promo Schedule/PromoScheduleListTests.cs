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
    }
}
