using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace CentroLink_Automation
{
    [TestFixture]
    public class AddMachineTests : BaseTest
    {
        private LoginPage loginPage;
        private MachineSetupPage machineSetup;
        private MachineDetailsPage addMachinePage;

        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            loginPage = new LoginPage(driver);
            machineSetup = new MachineSetupPage(driver);
            addMachinePage = new MachineDetailsPage(driver);

            await LotteryRetailDatabase.DeleteMachine(TestData.TestMachineNumber);
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.DeleteMachine(TestData.TestMachineNumber);
            await LotteryRetailDatabase.ResetTestMachine();
            await LotteryRetailDatabase.ResetTestBank();
        }


        [Test]
        public void GotoAddMachinePage()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            var options = addMachinePage.GetBankOptions();
            Assert.Greater(options.Count,0);
        }


        [Test]
        public void AddMachine_Default_Fields()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            Assert.True(string.IsNullOrEmpty(addMachinePage.GetMachineNumber()));
            Assert.True(string.IsNullOrEmpty(addMachinePage.GetLocationMachineNumber()));
            Assert.True(string.IsNullOrEmpty(addMachinePage.GetSerialNumber()));
            Assert.True(string.IsNullOrEmpty(addMachinePage.GetIPAddress()));
            Assert.True(string.IsNullOrEmpty(addMachinePage.GetDescription()));
        }


        [Test]
        public void AddMachine_Default_Game()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            string bank = addMachinePage.GetSelectedBank();
            Assert.True(string.IsNullOrEmpty(bank));

            string game = addMachinePage.GetSelectedGame();
            Assert.True(string.IsNullOrEmpty(game));
        }


        //Description automatically gets filled ot when a game is selected
        [Test]
        public void AddMachine_Description_SelectGame()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 0
            );

            string gameDescription = addMachinePage.GetSelectedGame();
            string description = addMachinePage.GetDescription();

            Assert.AreEqual(gameDescription, description);

        }



        [Test]
        public async Task AddMachine_GameIsEnabled_Default()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            bool isEnabled = addMachinePage.GameIsEnabled(0);
            Assert.True(isEnabled);
        }


        [Test]
        public async Task AddMachine_GameIsEnabled_MultiGame()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber,true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();
            Thread.Sleep(10000);
        }



        [Test]
        public void AddMachinePage_Back_Button()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();
            addMachinePage.ClickBackButton();

            Assert.True(addMachinePage.ConfirmationWindow.IsOpen);

            addMachinePage.ConfirmationWindow.Confirm();
            Assert.False(addMachinePage.ConfirmationWindow.IsOpen);

            int count = machineSetup.RowCount;
            Assert.Greater(count,0);
        }


        [Test]
        public void AddMachinePage_Back_Button_MachineNotAdded()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 0
            );

            addMachinePage.ReturnToMachineSetup();

            Assert.False(machineSetup.MachineFoundInList(TestData.TestMachineNumber.ToString()));
        }


        [Test]
        public void AddMachinePage_Back_Button_Cancel()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.ClickBackButton();
            Assert.True(addMachinePage.ConfirmationWindow.IsOpen);

            addMachinePage.ConfirmationWindow.Cancel();
            Assert.False(addMachinePage.ConfirmationWindow.IsOpen);
        }


        [Test]
        public void AddMachine_Success()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.SuccessWindow.IsOpen);
            addMachinePage.SuccessWindow.Confirm();

            Assert.True(machineSetup.MachineFoundInList(TestData.TestMachineNumber));
        }


        [Test]
        public void AddMachine_Game_NotSelected()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 -1
            );

            addMachinePage.Save();

            Assert.False(addMachinePage.SuccessWindow.IsOpen);
            Assert.True(addMachinePage.GameErrorIsDisplayed());
        }


        [Test]
        public void AddMachine_Success_Alert()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 0
            );

            Assert.False(addMachinePage.SuccessWindow.IsOpen);

            addMachinePage.Save();

            Assert.True(addMachinePage.SuccessWindow.IsOpen);
        }


        //User is taken back to machine setup screen if they press the OK butt on the success alet
        [Test]
        public void AddMachine_Success_Alert_Confirm()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 0
            );

            addMachinePage.Save();
            addMachinePage.SuccessWindow.Confirm();

            Assert.Greater(machineSetup.RowCount, 0);
            
        }



    }
}
