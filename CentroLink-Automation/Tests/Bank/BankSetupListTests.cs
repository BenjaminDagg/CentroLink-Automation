using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CentroLink_Automation
{
    public class BankSetupListTests : BaseTest
    {



        [SetUp]
        public async Task Setup()
        {

        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestDeal();
            await LotteryRetailDatabase.ExecuteRecommendDealForClosePrecedure();

            foreach (var deal in DealsToReset)
            {
                await LotteryRetailDatabase.UpdateDealEnabled(deal, true);
            }
        }
    }
}
