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

        public LoginPage loginPage;

        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            loginPage = new LoginPage(driver);
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();
        }
    }
}
