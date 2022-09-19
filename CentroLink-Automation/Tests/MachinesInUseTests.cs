using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

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

            await LotteryRetailDatabase.ResetTestMachine();
            await TpService.Connect();
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestMachine();

            TpService.Disconnect();
        }

        [Test]
        public async Task MachinesInUse_GoT_Page()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            Thread.Sleep(5000);
        }


        [Test]
        public async Task MachinesInUse_Status_SetOffline()
        {
            
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            int machineNo = int.Parse(TestData.DefaultMachineNumber);

            var machineBefore = machinesInUsePage.GetMachine(machineNo);
            Assert.NotNull(machineBefore);

            Assert.True(machineBefore.Status);

            TpService.SetOffline();

            Thread.Sleep(11000);

            var machineAfter = machinesInUsePage.GetMachine(machineNo);
            Assert.NotNull(machineAfter);

            Assert.False(machineAfter.Status);

        }


        [Test]
        public async Task MachinesInUse_Status_SetOnline()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            int machineNo = int.Parse(TestData.DefaultMachineNumber);

            var machine = machinesInUsePage.GetMachine(machineNo);
            Assert.NotNull(machine);

            Assert.True(machine.Status);

            TpService.SetOffline();

            Thread.Sleep(11000);

            machine = machinesInUsePage.GetMachine(machineNo);
            Assert.NotNull(machine);

            Assert.False(machine.Status);

            TpService.SetOnline();

            Thread.Sleep(11000);

            machine = machinesInUsePage.GetMachine(machineNo);
            Assert.NotNull(machine);

            Assert.True(machine.Status);

        }


        //Machine Balance should decrease after playing a losing game
        [Test]
        public async Task MachinesInUse_MachineBalance_Loss()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            int machineNo = int.Parse(TestData.DefaultMachineNumber);

            var machine = machinesInUsePage.GetMachine(machineNo);
            Assert.NotNull(machine);

            int machineBalanceCreditsBefore = (int)machine.Balance * 100;
            int betAmount = TpService.BetAmount;

            TpService.PlayLosingGame();
            Thread.Sleep(11000);

            machine = machinesInUsePage.GetMachine(machineNo);
            int machineBalanceCreditsAfter = (int)machine.Balance * 100;
            int expectedBalance = machineBalanceCreditsBefore - betAmount;
            Assert.AreEqual(expectedBalance, machineBalanceCreditsAfter);
        }


        [Test]
        public async Task MachinesInUse_MachineBalance_Win()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            int machineNo = int.Parse(TestData.DefaultMachineNumber);

            var machine = machinesInUsePage.GetMachine(machineNo);
            Assert.NotNull(machine);

            int machineBalanceCreditsBefore = (int)machine.Balance * 100;
            int betAmount = TpService.BetAmount;
            int winAmount = 0;

            TpService.PlayWinningGame(out winAmount);
            Thread.Sleep(11000);

            machine = machinesInUsePage.GetMachine(machineNo);

            int machineBalanceCreditsAfter = (int)machine.Balance * 100;
            machineBalanceCreditsAfter = (int)Math.Round(machineBalanceCreditsAfter / 100d,0,MidpointRounding.AwayFromZero) * 100;

            int expectedBalance = (machineBalanceCreditsBefore - betAmount) + winAmount;
            expectedBalance = (int)Math.Round(expectedBalance / 100d, 0, MidpointRounding.AwayFromZero) * 100;

            Assert.AreEqual(expectedBalance, machineBalanceCreditsAfter);
        }


        [Test]
        public async Task MachinesInUse_LastPlay()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            int machineNo = int.Parse(TestData.DefaultMachineNumber);

            var machine = machinesInUsePage.GetMachine(machineNo);
            Assert.NotNull(machine);

            DateTime lastPlayBefore = machine.LastPlay;

            TpService.PlayGame();
            Thread.Sleep(11000);

            machine = machinesInUsePage.GetMachine(machineNo);
            Assert.NotNull(machine);

            DateTime lastPlayAfter = machine.LastPlay;
            
            Assert.Greater(lastPlayAfter, lastPlayBefore);
        }


        //machine with $0 balance shouldn't be in list
        [Test]
        public async Task MachinesInUse_CashOut()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            Assert.True(machinesInUsePage.MachineFoundInList(TestData.DefaultMachineNumber));

            TpService.CashOut();
            Thread.Sleep(11000);

            Assert.False(machinesInUsePage.MachineFoundInList(TestData.DefaultMachineNumber));
        }


        [Test]
        public async Task MachinesInUse_Description()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            string expectedDesc = await LotteryRetailDatabase.GetMachineDescription(TestData.DefaultMachineNumber);

            var machine = machinesInUsePage.GetMachine(int.Parse(TestData.DefaultMachineNumber));

            Assert.AreEqual(expectedDesc, machine.Description);
        }


        [Test]
        public async Task MachinesInUse_InsertCash()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            Assert.True(machinesInUsePage.MachineFoundInList(TestData.DefaultMachineNumber));

            TpService.CashOut();
            Thread.Sleep(11000);

            Assert.False(machinesInUsePage.MachineFoundInList(TestData.DefaultMachineNumber));

            TpService.InsertCash(1);
            Thread.Sleep(11000);

            var machine = machinesInUsePage.GetMachine(int.Parse(TestData.DefaultMachineNumber));
            Assert.NotNull(machine);
  
            Assert.AreEqual(1.00,machine.Balance);
        }


        [Test]
        public async Task MachinesInUse_AddCash()
        {

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            var machine = machinesInUsePage.GetMachine(int.Parse(TestData.DefaultMachineNumber));
            Assert.NotNull(machine);

            int balanceBeforeCredits = (int)machine.Balance * 100;

            TpService.InsertCash(1);
            Thread.Sleep(11000);

            machine = machinesInUsePage.GetMachine(int.Parse(TestData.DefaultMachineNumber));
            Assert.NotNull(machine);

            int balanceAfterCredits = (int)(machine.Balance * 100);
            int expectedBalance = balanceBeforeCredits + 100;

            Assert.AreEqual(expectedBalance, balanceAfterCredits);
        }


        [Test]
        public void MachineInUse_Sort_Asc()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            //Machine Number
            machinesInUsePage.SortGridByHeaderAscending(0);
            var col1 = machinesInUsePage.GetValuesForColumn(0);
            var expectedCol1 = col1.Select(x => int.Parse(x)).OrderBy(x => x).ToList();

            for (int i = 0; i < col1.Count; i++)
            {
                Assert.AreEqual(expectedCol1[i], int.Parse(col1[i]));

            }

            //status
            machinesInUsePage.SortGridByHeaderAscending(1);
            var col2 = machinesInUsePage.GetValuesForColumn(1);
            var expectedCol2 = col2.Select(x => x).OrderBy(x => x).ToList();

            for (int i = 0; i < col2.Count; i++)
            {
                Assert.AreEqual(expectedCol2[i], col2[i]);

            }

            //Description
            machinesInUsePage.SortGridByHeaderAscending(2);
            var col3 = machinesInUsePage.GetValuesForColumn(2);
            var expectedCol3 = col3.Select(x => x).OrderBy(x => x).ToList();

            for (int i = 0; i < col3.Count; i++)
            {
                Assert.AreEqual(expectedCol3[i], col3[i]);

            }

            //Balance
            machinesInUsePage.SortGridByHeaderAscending(3);
            var col4 = machinesInUsePage.GetValuesForColumn(3);
            var expectedCol4 = col4.Select(x => double.Parse(x, System.Globalization.NumberStyles.Currency)).OrderBy(x => x).ToList();

            for (int i = 0; i < col4.Count; i++)
            {
                Assert.AreEqual(expectedCol4[i], double.Parse(col4[i], System.Globalization.NumberStyles.Currency));

            }

            //Promo balance
            machinesInUsePage.SortGridByHeaderAscending(4);
            var col5 = machinesInUsePage.GetValuesForColumn(4);
            var expectedCol5 = col5.Select(x => double.Parse(x, System.Globalization.NumberStyles.Currency)).OrderBy(x => x).ToList();

            for (int i = 0; i < col5.Count; i++)
            {
                Assert.AreEqual(expectedCol5[i], double.Parse(col5[i], System.Globalization.NumberStyles.Currency));

            }

            //Last Play
            machinesInUsePage.SortGridByHeaderAscending(5);
            var colDate = machinesInUsePage.GetValuesForColumn(5);
            var expectedColDate = colDate.Select(x => DateTime.ParseExact(x, "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)).OrderBy(x => x).ToList();

            for (int i = 0; i < colDate.Count; i++)
            {
                Assert.AreEqual(expectedColDate[i], DateTime.ParseExact(colDate[i], "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));

            }
        }


        [Test]
        public void MachineInUse_Sort_Desc()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachinesInUseTab();

            //Machine Number
            machinesInUsePage.SortGridByHeaderDescending(0);
            var col1 = machinesInUsePage.GetValuesForColumn(0);
            var expectedCol1 = col1.Select(x => int.Parse(x)).OrderByDescending(x => x).ToList();

            for (int i = 0; i < col1.Count; i++)
            {
                Assert.AreEqual(expectedCol1[i], int.Parse(col1[i]));

            }

            //status
            machinesInUsePage.SortGridByHeaderDescending(1);
            var col2 = machinesInUsePage.GetValuesForColumn(1);
            var expectedCol2 = col2.Select(x => x).OrderByDescending(x => x).ToList();

            for (int i = 0; i < col2.Count; i++)
            {
                Assert.AreEqual(expectedCol2[i], col2[i]);

            }

            //Description
            machinesInUsePage.SortGridByHeaderDescending(2);
            var col3 = machinesInUsePage.GetValuesForColumn(2);
            var expectedCol3 = col3.Select(x => x).OrderByDescending(x => x).ToList();

            for (int i = 0; i < col3.Count; i++)
            {
                Assert.AreEqual(expectedCol3[i], col3[i]);

            }

            //Balance
            machinesInUsePage.SortGridByHeaderDescending(3);
            var col4 = machinesInUsePage.GetValuesForColumn(3);
            var expectedCol4 = col4.Select(x => double.Parse(x, System.Globalization.NumberStyles.Currency)).OrderByDescending(x => x).ToList();

            for (int i = 0; i < col4.Count; i++)
            {
                Assert.AreEqual(expectedCol4[i], double.Parse(col4[i], System.Globalization.NumberStyles.Currency));

            }

            //Promo balance
            machinesInUsePage.SortGridByHeaderDescending(4);
            var col5 = machinesInUsePage.GetValuesForColumn(4);
            var expectedCol5 = col5.Select(x => double.Parse(x, System.Globalization.NumberStyles.Currency)).OrderByDescending(x => x).ToList();

            for (int i = 0; i < col5.Count; i++)
            {
                Assert.AreEqual(expectedCol5[i], double.Parse(col5[i], System.Globalization.NumberStyles.Currency));

            }

            //Last Play
            machinesInUsePage.SortGridByHeaderDescending(5);
            var colDate = machinesInUsePage.GetValuesForColumn(5);
            var expectedColDate = colDate.Select(x => DateTime.ParseExact(x, "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)).OrderByDescending(x => x).ToList();

            for (int i = 0; i < colDate.Count; i++)
            {
                Assert.AreEqual(expectedColDate[i], DateTime.ParseExact(colDate[i], "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));

            }
        }
    }
}
