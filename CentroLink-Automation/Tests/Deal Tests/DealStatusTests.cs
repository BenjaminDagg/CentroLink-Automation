using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CentroLink_Automation
{
    public class DealStatusTests : BaseTest
    {
        private LoginPage loginPage;
        private DealStatusPage dealStatusPage;
        private List<int> DealsToReset;

        [SetUp]
        public async Task Setup()
        {
            loginPage = new LoginPage(driver);
            dealStatusPage = new DealStatusPage(driver);

            DealsToReset = new List<int>();

            await LotteryRetailDatabase.AddTestDeal();
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestDeal();
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();

            foreach(var deal in DealsToReset)
            {
                await LotteryRetailDatabase.UpdateDealEnabled(deal, true);
            }
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

            Assert.False(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            await LotteryRetailDatabase.UpdateDealLastPlayMinusDays(-30,TestData.TestDealNumber);
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();

            dealStatusPage.Refresh();

            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));
        }


        [Test]
        public async Task DealStatus_RecommendForClose_PlayCount()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            Assert.False(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            await LotteryRetailDatabase.UpdateDealLastPlayMinusDays(-10, TestData.TestDealNumber);
            await LotteryRetailDatabase.UpdateDealPlayCountPercent(0.99, TestData.TestDealNumber);
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();

            dealStatusPage.Refresh();

            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));
        }


        [Test]
        public async Task DealStatus_RecommendForClose_DealClosed()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            Assert.False(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            await LotteryRetailDatabase.UpdateDealEnabled(TestData.TestDealNumber,false);
            await LotteryRetailDatabase.UpdateDealLastPlayMinusDays(-10, TestData.TestDealNumber);
            await LotteryRetailDatabase.UpdateDealPlayCountPercent(0.99, TestData.TestDealNumber);
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();

            dealStatusPage.Refresh();

            Assert.False(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Closed);

            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));
        }


        [Test]
        public async Task DealStatus_RecommendForClose_Status()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.All);

            var dealBefore = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.AreNotEqual(0, dealBefore.DealNumber);
            Assert.False(dealBefore.RecommendedToClose);

            await LotteryRetailDatabase.UpdateDealLastPlayMinusDays(-30, TestData.TestDealNumber);
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();

            dealStatusPage.Refresh();
            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.RecommendedForClose);

            var dealAfter = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.AreNotEqual(0, dealAfter.DealNumber);
            Assert.True(dealAfter.RecommendedToClose);
        }


        //All Deals in the Recommended For Close filter should have Recommend Close set to Yes
        [Test]
        public async Task DealStatus_RecommendForClose_Column()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.All);

            var dealBefore = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.AreNotEqual(0, dealBefore.DealNumber);

            await LotteryRetailDatabase.UpdateDealLastPlayMinusDays(-30, TestData.TestDealNumber);
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();

            dealStatusPage.Refresh();
            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.RecommendedForClose);

            var recommendCloseCol = dealStatusPage.GetValuesForColumn(2);
            recommendCloseCol.ForEach(val => Assert.AreEqual("Yes",val));
        }


        //All deals in the should should have status Open when the All Open deals filter is selected
        [Test]
        public async Task DealStatus_AllOpenDeals_Status()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Open);

            var statusCol = dealStatusPage.GetValuesForColumn(1);
            statusCol.ForEach(val => Assert.AreEqual("Open", val));
        }


        //Deals recommended to close are visible in the all open deals filter
        [Test]
        public async Task DealStatus_AllOpenDeals_RecommendCloseDeals()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Open);
            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            //set to recommend close
            await LotteryRetailDatabase.UpdateDealLastPlayMinusDays(-30, TestData.TestDealNumber);
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.RecommendedForClose);
            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Open);
            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));
        }


        [Test]
        public async Task DealStatus_AllOpenDeals_CloseDeal()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Open);
            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            //set to recommend close
            dealStatusPage.SelectRowByDealNumber(TestData.TestDealNumber);
            dealStatusPage.CloseDeal();
            dealStatusPage.Refresh();

            Assert.False(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Closed);
            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));
        }


        //Deals that are 'active unplayed' (active but play count is zero) should appear in the AllOpenDeals filter
        [Test]
        public async Task DealStatus_AllOpenDeals_ActiveUnplayedDeals()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            //recommended for close deal shouldnt be visible
            Assert.False(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            //all open deals deal should be visible
            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Open);
            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            //set deal play count to zero
            await LotteryRetailDatabase.UpdateDealPlayCount(0, TestData.TestDealNumber);
            dealStatusPage.Refresh();

            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));
            Thread.Sleep(5000);
        }

        #region Closed Deals

        [Test]
        public async Task DealStatus_ClosedDeals_Status()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Closed);

            var statusCol = dealStatusPage.GetValuesForColumn(1);
            statusCol.ForEach(val => Assert.AreEqual("Closed", val));
        }


        [Test]
        public async Task DealStatus_ClosedDeals_Status_Column()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.All);

            var dealBefore = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.AreNotEqual(0, dealBefore.DealNumber);
            Assert.True(dealBefore.IsOpen);

            dealStatusPage.SelectRowByDealNumber(TestData.TestDealNumber);
            dealStatusPage.CloseDeal();
            dealStatusPage.Refresh();

            var dealAfter = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.AreNotEqual(0, dealAfter.DealNumber);
            Assert.False(dealAfter.IsOpen);
        }


        [Test]
        public async Task DealStatus_ClosedDeals_ChangeDealStatus()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            //deal open visible
            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Open);
            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            //deal open - not visible in closed deal filter
            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Closed);
            Assert.False(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            //close deal
            await LotteryRetailDatabase.UpdateDealEnabled(TestData.TestDealNumber,false);
            dealStatusPage.Refresh();

            //deal not visible in closed filter
            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            //reopen deal - deal not visible in closed filter
            await LotteryRetailDatabase.UpdateDealEnabled(TestData.TestDealNumber, true);
            dealStatusPage.Refresh();

            Assert.False(dealStatusPage.DealFoundInList(TestData.TestDealNumber));
        }


        #endregion

        #region All Deals Filter

        //Open deals should appear in the list when All Deals is selected
        [Test]
        public async Task DealStatus_AllDealsFilter_OpenDeals()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Open);

            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.All);

            var deal = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.AreNotEqual(0, deal.DealNumber);
            Assert.True(deal.IsOpen);
        }


        //Closed deals should appear in the list when All Deals is selected
        [Test]
        public async Task DealStatus_AllDealsFilter_ClosedDeals()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Open);

            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));
            dealStatusPage.SelectRowByDealNumber(TestData.TestDealNumber);
            dealStatusPage.CloseDeal();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.All);
            dealStatusPage.Refresh();

            var deal = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.AreNotEqual(0, deal.DealNumber);
            Assert.False(deal.IsOpen);
        }


        //deals recommend to close should appear in the list when All Deals is selected
        [Test]
        public async Task DealStatus_AllDealsFilter_DealsRecommendToClose()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            await LotteryRetailDatabase.UpdateDealLastPlayMinusDays(-30,TestData.TestDealNumber);
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();

            dealStatusPage.Refresh();
            Assert.True(dealStatusPage.DealFoundInList(TestData.TestDealNumber));

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.All);

            var deal = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.AreNotEqual(0, deal.DealNumber);
            Assert.True(deal.IsOpen);
        }

        #endregion


        [Test]
        public async Task DealStatus_DealCompleted()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.All);

            var deal = dealStatusPage.GetDealByDealNumber(TestData.TestDealNumber);
            Assert.AreNotEqual(0,deal.DealNumber);

            double expectedPercentComplete = ((double)deal.TabsDispensed / (double)deal.TabsPerDeal) * 100;
            double percent = Math.Round(expectedPercentComplete, 2);
            var actualPercentComplete = deal.Completed;

            Assert.AreEqual(percent, actualPercentComplete);
        }


        [Test]
        public async Task DealStatus_Close_Multiple()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickDealStatusTab();

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Open);
            dealStatusPage.SortGridByHeaderDescending(0);

            var testDeals = new List<Deal>();
            testDeals.Add(dealStatusPage.GetDealAtRowNum(0));
            testDeals.Add(dealStatusPage.GetDealAtRowNum(1));
            testDeals.Add(dealStatusPage.GetDealAtRowNum(2));

            DealsToReset = testDeals.Select(deal => deal.DealNumber).ToList();

            testDeals.ForEach(deal => Assert.True(deal.IsOpen));

            dealStatusPage.SelectRows(0, 1, 2);
            dealStatusPage.CloseDeal();

            testDeals.ForEach(deal => Assert.False(dealStatusPage.DealFoundInList(deal.DealNumber)));

            dealStatusPage.SelectDealStatusFilter(DealStatusPage.DealStatus.Closed);
            dealStatusPage.SortGridByHeaderDescending(0);
            testDeals.ForEach(deal => Assert.True(dealStatusPage.DealFoundInList(deal.DealNumber)));
        }
    }
}