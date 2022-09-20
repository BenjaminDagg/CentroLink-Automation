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
        }


        [Test]
        public void GoTo_Page()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickPromoTicketSetupTab();

            promoList.TurnPromoTicketsOn();
            Thread.Sleep(3000);
            promoList.TurnPromoTicketsOff();
            Thread.Sleep(3000);
            promoList.TurnPromoTicketsOn();
            Thread.Sleep(3000);
        }
    }
}
