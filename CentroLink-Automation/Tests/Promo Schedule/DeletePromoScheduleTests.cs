using NUnit.Framework;
using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;


namespace CentroLink_Automation
{
    public class DeletePromoScheduleTests : BaseTest
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
        public async Task DeletePromo_NotStarted()
        {
            string startDateString = DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string endDateString = DateTime.Now.AddHours(3).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            await LotteryRetailDatabase.UpdatePromo(TestData.TestPromoEntryScheduleId, startDate, endDate, false, false);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);
            promoList.ClickDeletePromo();

            
            string alertText = promoList.DeletePromoAlert.AlertText;
            string expectedText = $"Are you sure that you want to delete Promo Schedule Id: {TestData.TestPromoEntryScheduleId}";
            
            Assert.True(alertText.Contains(expectedText));
        }


        [Test]
        public async Task DeletePromo_Promo_Ended()
        {
            string startDateString = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string endDateString = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            await LotteryRetailDatabase.UpdatePromo(TestData.TestPromoEntryScheduleId, startDate, endDate, false, false);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);
            promoList.ClickDeletePromo();


            string alertText = promoList.DeletePromoAlert.AlertText;
            string expectedText = $"Finished schedules cannot be deleted";

            Assert.True(alertText.Contains(expectedText));
        }


        [Test]
        public async Task DeletePromo_Promo_InProgress()
        {
            string startDateString = DateTime.Now.ToString("yyyy-MM-dd HH:00:00.000");
            DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            string endDateString = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:00:00.000");
            DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            await LotteryRetailDatabase.UpdatePromo(TestData.TestPromoEntryScheduleId, startDate, endDate, false, false);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.SelectRowByPromoId(TestData.TestPromoEntryScheduleId);
            promoList.ClickDeletePromo();

            string alertText = promoList.DeletePromoAlert.AlertText;
            string expectedText = $"Active schedules cannot be deleted. Schedule can be modified to disable promotion as soon as possible. Would you like to stop the current promotion?";

            Assert.AreEqual(expectedText,alertText);
        }


        [Test]
        public void DeletePromo_Success()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();

            string description = "Test Delete Promo";
            string startDate = ParseDateString(DateTime.Now.AddHours(1));
            string endDate = ParseDateString(DateTime.Now.AddHours(2));

            PromoToDelete = new PromoEntrySchedule { Description = description };

            addPromo.EnterDescription(description);
            addPromo.EnterPromoStartDate(startDate);
            addPromo.EnterPromoEndDate(endDate);

            addPromo.Save();

            Assert.True(promoList.PromoFoundInList(description));

            var promo = promoList.GetPromoSchedule(description);
            Assert.NotNull(promo);

            promoList.SelectRowByPromoId(promo.Id);
            promoList.DeleteSelectedPromo();

            Assert.False(promoList.PromoFoundInList(promo.Id));

        }


        [Test]
        public void DeletePromo_Cancel()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();

            string description = "Test Delete Promo";
            string startDate = ParseDateString(DateTime.Now.AddHours(1));
            string endDate = ParseDateString(DateTime.Now.AddHours(2));

            PromoToDelete = new PromoEntrySchedule { Description = description };

            addPromo.EnterDescription(description);
            addPromo.EnterPromoStartDate(startDate);
            addPromo.EnterPromoEndDate(endDate);

            addPromo.Save();

            Assert.True(promoList.PromoFoundInList(description));

            var promo = promoList.GetPromoSchedule(description);
            Assert.NotNull(promo);

            promoList.SelectRowByPromoId(promo.Id);
            promoList.ClickDeletePromo();
            promoList.DeletePromoAlert.Cancel();

            Assert.True(promoList.PromoFoundInList(promo.Id));

        }


        [Test]
        public async Task DeletePromo_EndPromoInProgress()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.ClickAddPromo();

            string description = "Test Delete Promo";
            string startDate = ParseDateString(DateTime.Now.AddHours(1));
            string endDate = ParseDateString(DateTime.Now.AddHours(2));

            PromoToDelete = new PromoEntrySchedule { Description = description };

            addPromo.EnterDescription(description);
            addPromo.EnterPromoStartDate(startDate);
            addPromo.EnterPromoEndDate(endDate);

            addPromo.Save();

            Assert.True(promoList.PromoFoundInList(description));

            var promoBefore = promoList.GetPromoSchedule(description);

            await LotteryRetailDatabase.UpdatePromo(promoBefore.Id, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(2), true, false);
            string expectedEndDate = DateTime.Now.ToString("M/dd/yyyy h:mm:00 tt");
            promoList.RefreshList();

            promoBefore = promoList.GetPromoSchedule(promoBefore.Id);

            promoList.SelectRowByPromoId(promoBefore.Id);
            promoList.DeleteSelectedPromo();

            var promoAfter = promoList.GetPromoSchedule(promoBefore.Id);
            string actualEndDate = promoAfter.EndTime.ToString("M/dd/yyyy h:mm:00 tt");
            
            Assert.AreEqual(expectedEndDate,actualEndDate);
        }
    }
}
