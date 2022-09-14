using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using CentroLink_Automation.Pages.Machine_Setup;
using System.Collections.Generic;

namespace CentroLink_Automation
{
    public class DuplicateMachineTests : BaseTest
    {

        private LoginPage loginPage;
        private MachineSetupPage machineSetup;
        private DuplicateMachinePage dupeMachinePage;
        private EditMachinePage editMachinePage;
        private string MachineIdToDelete = TestData.TestMachineNumber;

        [SetUp]
        public void Setup()
        {
            MachineIdToDelete = TestData.TestMachineNumber;

            loginPage = new LoginPage(driver);
            machineSetup = new MachineSetupPage(driver);
            dupeMachinePage = new DuplicateMachinePage(driver);
            editMachinePage = new EditMachinePage(driver);
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestMachine();
            await LotteryRetailDatabase.DeleteMachine(TestData.TestMachineNumber);
            await LotteryRetailDatabase.DeleteMachine(MachineIdToDelete);
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
        public void DuplicateMachine_Successful()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            string machineId = dupeMachinePage.GetMachineNumber();
            this.MachineIdToDelete = machineId;

            dupeMachinePage.EnterForm(
                machineId,
                TestData.TestLocationMachineNumber,
                TestData.TestMachineSerialNumber,
                TestData.TestMachineIpAddress
            );

            dupeMachinePage.Save();
            Assert.IsTrue(dupeMachinePage.SuccessWindow.IsOpen);

            dupeMachinePage.SuccessWindow.Confirm();
            Assert.True(machineSetup.MachineFoundInList(machineId));

        }


        [Test]
        public void DuplicateMachine_NextMachineNumber()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            var machNos = machineSetup.GetValuesForColumn(0).Select(val => int.Parse(val)).ToList();
            int startMachNo = int.Parse(TestData.DefaultMachineNumber);

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            int expected = MachineSetupPage.NextAvailableMachineNumber(machNos, startMachNo);
            int actual = int.Parse(dupeMachinePage.GetMachineNumber());

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DuplicateMachine_MachineNumberToDuplicate()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            var expected = TestData.DefaultMachineNumber;
            var actual = dupeMachinePage.GetMachineNumberToDuplicate();

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DuplicateMachine_MachineNumberToDuplicate_ReadyOnly()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            Assert.True(dupeMachinePage.IsReadOnly(MachineFields.MachineNumberToDuplicate));
        }


        //if selected machine Locatin Machine number is numeric, then the next available
        //number is automatically selected
        [Test]
        public void DuplicateMachine_LocationMachineNumber_NextAvailable_Numeric()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            var machNos = new List<int>();
            foreach(var val in machineSetup.GetValuesForColumn(1))
            {
                Console.WriteLine(val);
                try
                {
                    machNos.Add(int.Parse(val));
                }
                catch(Exception ex)
                {

                }
            }
            int startMachNo = int.Parse(TestData.DefaultLocationMachineNumber);

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            int expected = MachineSetupPage.NextAvailableMachineNumber(machNos, startMachNo);
            Console.WriteLine(expected);
            int actual = int.Parse(dupeMachinePage.GetLocationMachineNumber());

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DuplicateMachine_LocationMachineNumber_NextAvailable_AlphaNumeric()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            editMachinePage.EnterLocationMachineNumber("12b!@");
            editMachinePage.Save();
            editMachinePage.SuccessWindow.Confirm();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            string locMachNo = dupeMachinePage.GetLocationMachineNumber();
            Assert.True(string.IsNullOrEmpty(locMachNo));
        }


        [Test]
        public void DuplicateMachine_LocationMachineNumber_Prepopulated()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            string locMachNo = dupeMachinePage.GetLocationMachineNumberToDuplicate();
            Assert.AreEqual(TestData.DefaultLocationMachineNumber, locMachNo);
        }

        [Test]
        public void DuplicateMachine_LocationMachineNumber_Readonly()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            Assert.True(dupeMachinePage.IsReadOnly(MachineFields.LocationMachineNumberToDuplicate));
        }


        [Test]
        public void DuplicateMachine_Description_Blank()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            var description = dupeMachinePage.GetDescription();
            Assert.True(string.IsNullOrEmpty(description));
        }


        [Test]
        public void DuplicateMachine_SerialNumber_Blank()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            var sn = dupeMachinePage.GetSerialNumber();
            Assert.True(string.IsNullOrEmpty(sn));
        }


        [Test]
        public void DuplicateMachine_IpAddress_NextAvailable()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            var ips = machineSetup.GetValuesForColumn(4);
            string expected = MachineSetupPage.NextAvailableIpAddress(ips,TestData.DefaultIPAddress);

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            string actual = dupeMachinePage.GetIPAddress();
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public async Task DuplicateMachine_Copy_Games()
        {
            
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            int gameCount = editMachinePage.RowCount;
            Assert.Greater(gameCount, 0);

            string expectedBank = editMachinePage.GetSelectedBank(0);
            string expectedGame = editMachinePage.GetSelectedGame(0);

            editMachinePage.ReturnToMachineSetup();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            Assert.AreEqual(gameCount,dupeMachinePage.RowCount);
            Assert.AreEqual(expectedBank,dupeMachinePage.GetSelectedBank());
            Assert.AreEqual(expectedGame, dupeMachinePage.GetSelectedGame());

            dupeMachinePage.EnterForm(
                TestData.TestMachineNumber,
                TestData.TestLocationMachineNumber,
                TestData.TestMachineSerialNumber,
                TestData.TestMachineIpAddress
            );

            dupeMachinePage.Save();
            Assert.True(dupeMachinePage.SuccessWindow.IsOpen);
        }


        [Test]
        public async Task DuplicateMachine_Assign_Game()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            int gameCount = editMachinePage.RowCount;
            Assert.Greater(gameCount, 0);

            string expectedBank = editMachinePage.GetSelectedBank(0);
            string expectedGame = editMachinePage.GetSelectedGame(0);

            editMachinePage.ReturnToMachineSetup();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickDuplicateMachine();

            Assert.AreEqual(gameCount, dupeMachinePage.RowCount);

            dupeMachinePage.EnterForm(
                TestData.TestMachineNumber,
                TestData.TestLocationMachineNumber,
                TestData.TestMachineSerialNumber,
                TestData.TestMachineIpAddress
            );

            dupeMachinePage.Save();
            dupeMachinePage.SuccessWindow.Confirm();

            var assignedGames = await LotteryRetailDatabase.GetGamesAssignedToMachine(TestData.TestMachineNumber);
            
            Assert.AreEqual(gameCount,assignedGames.Count);
            var game = assignedGames.Where(game => expectedGame.Contains(game.GameCode)).ToList();
            Assert.AreEqual(1,game.Count);
        }

    }
}
