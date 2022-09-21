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


        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            loginPage = new LoginPage(driver);
            promoList = new PromoScheduleListPage(driver);
            addPromo = new AddPromoSchedulePage(driver);
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestPromo();
        }
    }
}
