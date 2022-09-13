using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace CentroLink_Automation
{
    [TestFixture]
    public class MachineSetupValidationTests : BaseTest
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
        public void AddMachine_MachNo_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 "",
                 "356653",
                 "35545333",
                 "65.75.82.219",
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.MachineNumberErrorIsDisplayed());
        }


        [Test]
        public void AddMachine_MachNo_MaxLength()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 "123456",
                 "356653",
                 "35545333",
                 "65.75.82.219",
                 0,
                 0
            );

            addMachinePage.Save();

            string machNo = addMachinePage.GetMachineNumber();
            Assert.AreEqual(5, machNo.Length);
        }


        [Test]
        [TestCase("123ab")]
        [TestCase("123!@")]
        public void AddMachine_MachNo_SpecialCharacters(string machNo)
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 machNo,
                 "356653",
                 "35545333",
                 "65.75.82.219",
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.MachineNumberErrorIsDisplayed());
        }


        [Test]
        public void AddMachine_MachNo_Exists()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            var machine = machineSetup.GetMachineAtRow(1);
            Assert.NotNull(machine);

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 machine.MachineNumber,
                 "356653",
                 "35545333",
                 "65.75.82.219",
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.ErrorWindow.IsOpen);
        }


        [Test]
        public void AddMachine_LocMachNo_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            Assert.False(addMachinePage.LocationMachineNumberErrorIsDisplayed());

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 "",
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.LocationMachineNumberErrorIsDisplayed());
        }


        [Test]
        public void AddMachine_LocMachNo_MaxLength()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            Assert.False(addMachinePage.LocationMachineNumberErrorIsDisplayed());

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 "1234567890",
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 0
            );

            addMachinePage.Save();

            string locMachineNum = addMachinePage.GetLocationMachineNumber();
            Assert.AreEqual(8, locMachineNum.Length);
        }


        [Test]
        public void AddMachine_LocMachNo_Exists()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.DefaultLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.ErrorWindow.IsOpen);
        }


        [Test]
        [TestCase("abc123")]
        [TestCase("a1!@#D<")]
        public void AddMachine_LocMachNo_Alphanumeric(string locMachineNum)
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 locMachineNum,
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.SuccessWindow.IsOpen);
        }



        [Test]
        public void AddMachine_Description_Empty()
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

            //delete description text
            addMachinePage.EnterDescription("");

            addMachinePage.Save();

            Assert.True(addMachinePage.DescriptionErrorIsDisplayed());

        }


        [Test]
        public void AddMachine_Description_Max_Length()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            string expectedDescription = new string('a', 65);

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 0
            );

            addMachinePage.EnterDescription(expectedDescription);

            addMachinePage.Save();

            string actualDescription = addMachinePage.GetDescription();

            Assert.AreEqual(64, actualDescription.Length);

        }


        [Test]
        public void AddMachine_Description_Alphanumeric()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            string expectedDescription = "ABCabc123!@#<>";

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 0,
                 0
            );

            addMachinePage.EnterDescription(expectedDescription);

            addMachinePage.Save();

            Assert.True(addMachinePage.SuccessWindow.IsOpen);

        }


        [Test]
        public void AddMachine_IP_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            Assert.False(addMachinePage.IPAddressErrorIsDisplayed());

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 "",
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.IPAddressErrorIsDisplayed());

        }


        [Test]
        public void AddMachine_IP_Exists()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 TestData.DefaultIPAddress,
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.ErrorWindow.IsOpen);

        }


        [Test]
        public void AddMachine_IP_MaxLength()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 "111.111.111.111.1",
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.IPAddressErrorIsDisplayed());

        }


        [Test]
        [TestCase("111.111.111.256")] //max 255
        [TestCase("111.111.111.-1")]
        [TestCase("111.111.111")]   //3 octets
        [TestCase("111.111.111.111.111")] //5 octeets
        [TestCase("111.111.1111111")] //missing period
        [TestCase("111.111..111.111")]
        public void AddMachine_IP_Invalid(string ip)
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 ip,
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.IPAddressErrorIsDisplayed());
        }


        //Only numbers and decimals are allowed for IP Address
        [Test]
        [TestCase("123.abc.111.111")]
        [TestCase("123.12@.111.111")]
        public void AddMachine_IP_AlphanumericCharacters(string ip)
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 ip,
                 0,
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.IPAddressErrorIsDisplayed());

        }


        [Test]
        public void AddMachine_Bank_Empty()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.EnterForm(
                 TestData.TestMachineNumber,
                 TestData.TestLocationMachineNumber,
                 TestData.TestMachineSerialNumber,
                 TestData.TestMachineIpAddress,
                 -1,    //invalid index wont select a bank
                 0
            );

            addMachinePage.Save();

            Assert.True(addMachinePage.BankErrorIsDisplayed());
        }


        //bank dropdown has list of all active banks in the database
        [Test]
        public async Task AddMachine_Bank_Options()
        {
            var banks = await LotteryRetailDatabase.GetAllBanks(filterActive: true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            var bankOptions = addMachinePage.GetBankOptions();

            Assert.AreEqual(banks.Count, bankOptions.Count);
        }


        //Inactive banks are hidden in dropdown
        [Test]
        public async Task AddMachine_Bank_InActive()
        {
            var bank = await LotteryRetailDatabase.GetBankByBankNumber(TestData.TestBankNumber);
            Assert.NotNull(bank);

            string expected = $"{bank.BankNumber}: {bank.GameTypeCode} - {bank.Description}";

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            var bankOptions = addMachinePage.GetBankOptions();
            var match = bankOptions.FirstOrDefault(bank => bank == expected);

            Assert.NotNull(match);

            await LotteryRetailDatabase.UpdateBankIsActive(TestData.TestBankNumber, false);

            addMachinePage.ReturnToMachineSetup();
            machineSetup.ClickAddMachine();

            bankOptions = addMachinePage.GetBankOptions();
            match = bankOptions.FirstOrDefault(bank => bank == expected);

            Assert.Null(match);

        }


        [Test]
        public async Task AddMachine_Game_ForBank()
        {
            var bank = await LotteryRetailDatabase.GetBankByBankNumber(TestData.TestBankNumber);
            Assert.NotNull(bank);

            var games = await LotteryRetailDatabase.GetGamesForBankNumber(TestData.TestBankNumber);
            games.ForEach(game => Console.WriteLine(game.GameDescription));

            string expected = $"{bank.BankNumber}: {bank.GameTypeCode} - {bank.Description}";

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            addMachinePage.SelectBank(expected);

            var gameOptions = addMachinePage.GetGameOptions();
            Assert.Greater(gameOptions.Count, 0);

            Assert.AreEqual(games.Count, gameOptions.Count);

        }



        [Test]
        public void AddMachine_GameList_Columns()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.ClickAddMachine();

            Assert.AreEqual("Bank", addMachinePage.GetHeader(0));
            Assert.AreEqual("Game", addMachinePage.GetHeader(1));
            Assert.AreEqual("Enabled", addMachinePage.GetHeader(2));
        }

    }
}
