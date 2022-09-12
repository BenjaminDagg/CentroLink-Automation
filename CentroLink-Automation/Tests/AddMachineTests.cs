using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;

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
            Assert.AreEqual(5,machNo.Length);
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
        public async Task Test()
        {

            loginPage.Login("user1", "Diamond1#");
            navMenu.ClickMachineSetupTab();
            Console.WriteLine("machine setup count = " + machineSetup.RowCount);
            //machineSetup.ClickAddMachine();
            machineSetup.ClickAddMachine();

            addMachinePage.EnterMachineNumber("test");
            Console.WriteLine("You entered: " + addMachinePage.GetMachineNumber());
            /*
            addMachinePage.EnterMachineNumber("mNum");
            addMachinePage.EnterLocationMachineNumber("lmn");
            addMachinePage.EnterSerialNumber("SN");
            addMachinePage.EnterIPAddress("ip");
            addMachinePage.EnterDescription("Description");*/

            /*
            var banks = addMachinePage.GetBankOptions();
            foreach (var bank in banks)
            {
                Console.WriteLine(bank);
            }
            Console.WriteLine("test");
            addMachinePage.SelectBank(banks[1]);
            Console.WriteLine("You selected: " + addMachinePage.GetSelectedBank());
            

            addMachinePage.GetGameOptions().ForEach(game => Console.WriteLine(game));

            addMachinePage.GameDropdown.SelectByName("Blues N Bucks",false);
            Console.WriteLine("You selected: " + addMachinePage.GetSelectedGame());
            addMachinePage.Save();
            */

            /*
            var banks = addMachinePage.GetBankOptions(0);
            Console.WriteLine("FOund banks: ");
            foreach(var bank in banks)
            {
                Console.WriteLine(bank);    
            }
            addMachinePage.SelectBank(0, banks[1]);
            Console.WriteLine("Selected Bank: " + addMachinePage.GetSelectedBank(0));
            addMachinePage.GetGameOptions(0).ForEach(game => Console.WriteLine(game));
            addMachinePage.SelectGame(0, "9Y2 - American Dreams 50 Cent");
            Console.WriteLine("Selected game: " + addMachinePage.GetSelectedGame(0));
            */
        }


        
    }
}
