using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace CentroLink_Automation
{
    public class DealStatusTests : BaseTest
    {
        private LoginPage loginPage;
        private DealStatusPage dealStatusPage;

        [SetUp]
        public async Task Setup()
        {
            loginPage = new LoginPage(driver);
            dealStatusPage = new DealStatusPage(driver);

            await LotteryRetailDatabase.AddTestDeal();
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestDeal();
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();
        }

        [Test]
        public void Default_Status_Filter()
        {
            loginPage.Login(TestData.AdminUsername,TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            var currentStatus = dealStatusPage.GetSelectedFilter();
            Assert.AreEqual(DealStatusPage.DealStatus.RecommendedForClose, currentStatus);
        }


        [Test]
        public void Default_Status_Filter_Options()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            var options = dealStatusPage.DealStatusFilterOptions();

            Assert.True(options.Contains(DealStatusPage.DealStatus.RecommendedForClose));
            Assert.True(options.Contains(DealStatusPage.DealStatus.Open));
            Assert.True(options.Contains(DealStatusPage.DealStatus.Closed));
            Assert.True(options.Contains(DealStatusPage.DealStatus.All));
        }


        [Test]
        public void Default_Status_Columns()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            Assert.AreEqual("Deal Number", dealStatusPage.GetHeader(0));
            Assert.AreEqual("Status", dealStatusPage.GetHeader(1));
            Assert.AreEqual("Recommend Close", dealStatusPage.GetHeader(2));
            Assert.AreEqual("Description", dealStatusPage.GetHeader(3));
            Assert.AreEqual("Tab Amount", dealStatusPage.GetHeader(4));
            Assert.AreEqual("Tabs Dispensed", dealStatusPage.GetHeader(5));
            Assert.AreEqual("Tabs Per Deal", dealStatusPage.GetHeader(6));
            Assert.AreEqual("Completed", dealStatusPage.GetHeader(7));
            Assert.AreEqual("Last Play", dealStatusPage.GetHeader(8));
        }


        [Test]
        public void DealStatus_CloseDeal()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.All);

            var dealBefore = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.AreNotEqual(0,dealBefore.DealNumber);

            Assert.True(dealBefore.IsOpen);

            dealStatusPage.SelectRowByDealNumber(TestData.TestDealNumber);
            dealStatusPage.CloseDeal();
            dealStatusPage.Refresh();

            var dealAfter = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.False(dealAfter.IsOpen);
        }


        [Test]
        public void DealStatus_CloseDeal_Disabled_NoSelection()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            var closeBtn = driver.FindElement(dealStatusPage.CloseDealButton);

            Assert.False(closeBtn.Enabled);
        }


        [Test]
        public void DealStatus_CloseDeal_Disabled_DealAlreadyClosed()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.All);

            var dealBefore = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.AreNotEqual(0,dealBefore.DealNumber);

            if (dealBefore.IsOpen)
            {
                dealStatusPage.SelectRowByDealNumber(TestData.TestDealNumber);
                dealStatusPage.CloseDeal();
                dealStatusPage.Refresh();
            }

            dealStatusPage.SelectRowByDealNumber(TestData.TestDealNumber);
            var closeBtn = driver.FindElement(dealStatusPage.CloseDealButton);

            Assert.False(closeBtn.Enabled);
        }


        //Deal gets recomended for close if it is active and the last play in more than 1 month ago
        [Test]
        public async Task DealStatus_RecommendForClose_LastPlay()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            string dateMinus1MonthString = DateTime.Now.AddDays(-36).ToString("yyyy-MM-dd HH-mm-ss.fff");
            DateTime dateMinus1Month = DateTime.ParseExact(dateMinus1MonthString, "yyyy-MM-dd HH-mm-ss.fff", CultureInfo.InvariantCulture);

            Assert.False(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            await LotteryRetailDatabase.UpdateDealLastPlayMinusDays(-30,TestData.TestDealNumber);
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();

            dealStatusPage.Refresh();

            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));
        }
    }
}