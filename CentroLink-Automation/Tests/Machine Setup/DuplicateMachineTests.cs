using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;


namespace CentroLink_Automation
{
    public class DuplicateMachineTests : BaseTest
    {

        private LoginPage loginPage;
        private MachineSetupPage machineSetup;
        private DuplicateMachinePage dupeMachinePage;


        [SetUp]
        public void Setup()
        {
            loginPage = new LoginPage(driver);
            machineSetup = new MachineSetupPage(driver);
            dupeMachinePage = new DuplicateMachinePage(driver);
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestMachine();
            await LotteryRetailDatabase.DeleteMachine(TestData.TestMachineNumber);
        }


        [Test]
        public void DuplicateMachine_GoTo_Page()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            string machineToDuplicate = dupeMachinePage.GetMachineNumberToDuplicate();
            Assert.AreEqual(TestData.DefaultMachineNumber,machineToDuplicate);
            
        }


        [Test]
        public void NextAvailable()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            var machNos = machineSetup.GetValuesForColumn(0);
            
            var start = machNos[0];

            
        }
    }
}
