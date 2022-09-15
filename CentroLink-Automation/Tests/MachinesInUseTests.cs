using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;


namespace CentroLink_Automation
{
    public class MachinesInUseTests : BaseTest
    {

        private LoginPage loginPage;
        private MachinesInUsePage machinesInUsePage;

        [SetUp]
        public async Task Setup()
        {
            loginPage = new LoginPage(driver);
            machinesInUsePage = new MachinesInUsePage(driver);
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestMachine();
            await LotteryRetailDatabase.UpdateMachineLastPlay("00003");
        }

        [Test]
        public async Task MachinesInUse_GoT_Page()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            Thread.Sleep(5000);
        }


        [Test]
        public async Task MachinesInUse_Status()
        {

            await LotteryRetailDatabase.SetMachineInUse(TestData.DefaultMachineNumber);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            int machineNo = int.Parse(TestData.DefaultMachineNumber);

            var machineBefore = machinesInUsePage.GetMachine(machineNo);
            Assert.NotNull(machineBefore);

            Assert.True(machineBefore.Status);

            await LotteryRetailDatabase.UpdateMachineActivedFlag(TestData.DefaultMachineNumber,false);

            Thread.Sleep(5000);

            var machineAfter = machinesInUsePage.GetMachine(machineNo);
            Assert.NotNull(machineAfter);

            Assert.False(machineAfter.Status);

        }
    }
}
