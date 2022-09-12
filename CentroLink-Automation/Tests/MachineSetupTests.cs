using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace CentroLink_Automation
{
    [TestFixture]
    public class MachineSetupTests : BaseTest
    {
        private LoginPage loginPage;
        private MachineSetupPage machineSetup;


        [SetUp]
        public void Setup()
        {
            loginPage = new LoginPage(driver);
            machineSetup = new MachineSetupPage(driver);
        }


        [Test]
        public async Task Show_Hidden_Machines()
        {
            
            loginPage.Login("user1", "Diamond1#");
            navMenu.ClickMachineSetupTab();
            
            Assert.True(machineSetup.MachineFoundInList(TestData.MachineNumber));

            await LotteryRetailDatabase.UpdateMachineRemovedFlag(TestData.MachineNumber, true);

            machineSetup.ClickRefreshButton();

            Assert.False(machineSetup.MachineFoundInList(TestData.MachineNumber));

            machineSetup.ShowRemovedMachines();
            Assert.True(machineSetup.MachineFoundInList(TestData.MachineNumber));
        }


        [Test]
        public async Task Machine_Status_Active()
        {
            //set machine to Active in database
            await LotteryRetailDatabase.UpdateMachineActivedFlag(TestData.MachineNumber, true);

            loginPage.Login("user1", "Diamond1#");
            navMenu.ClickMachineSetupTab();

            Assert.True(machineSetup.MachineIsActive(TestData.MachineNumber));
        }


        [Test]
        public async Task Machine_Status_Offline()
        {
            //set machine to Offline in database
            await LotteryRetailDatabase.UpdateMachineActivedFlag(TestData.MachineNumber, false);

            loginPage.Login("user1", "Diamond1#");
            navMenu.ClickMachineSetupTab();

            Assert.False(machineSetup.MachineIsActive(TestData.MachineNumber));

            //set machine to Online in database
            await LotteryRetailDatabase.UpdateMachineActivedFlag(TestData.MachineNumber, true);
            machineSetup.ClickRefreshButton();

            Assert.True(machineSetup.MachineIsActive(TestData.MachineNumber));
        }


        [Test]
        public async Task Machine_Removed_Column()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            Assert.False(machineSetup.MachineIsRemoved(TestData.MachineNumber));

            //set machine to removed in the database
            await LotteryRetailDatabase.UpdateMachineRemovedFlag(TestData.MachineNumber, true);
            machineSetup.ClickRefreshButton();
            machineSetup.ShowRemovedMachines();

            Assert.True(machineSetup.MachineIsRemoved(TestData.MachineNumber));
        }


        [Test]
        public void Machine_Setup_Headers()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            Assert.AreEqual("Machine", machineSetup.GetHeader(0));
            Assert.AreEqual("Location Machine No", machineSetup.GetHeader(1));
            Assert.AreEqual("Description", machineSetup.GetHeader(2));
            Assert.AreEqual("Status", machineSetup.GetHeader(3));
            Assert.AreEqual("IP Address", machineSetup.GetHeader(4));
            Assert.AreEqual("Removed", machineSetup.GetHeader(5));
            Assert.AreEqual("Serial No", machineSetup.GetHeader(6));
            Assert.AreEqual("OS Version", machineSetup.GetHeader(7));
        }


        //Removed machines are hidden by default
        [Test]
        public void MachineSetup_ShowRemoved_Default()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            Assert.False(machineSetup.IsShowingRemoved);
        }


        [Test]
        public async Task MachineSetup_Refresh()
        {
            //set machine to Active in database
            await LotteryRetailDatabase.UpdateMachineActivedFlag(TestData.MachineNumber, true);

            loginPage.Login("user1", "Diamond1#");
            navMenu.ClickMachineSetupTab();

            Assert.True(machineSetup.MachineIsActive(TestData.MachineNumber));

            await LotteryRetailDatabase.UpdateMachineActivedFlag(TestData.MachineNumber, false);
            machineSetup.ClickRefreshButton();

            Assert.False(machineSetup.MachineIsActive(TestData.MachineNumber));
        }


        [Test]
        public async Task Test()
        {
            //set machine to Active in database
            await LotteryRetailDatabase.UpdateMachineActivedFlag(TestData.MachineNumber, true);

            loginPage.Login("user1", "Diamond1#");
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber("00001");
            machineSetup.ClickDuplicateMachine();
        }
    }
}
