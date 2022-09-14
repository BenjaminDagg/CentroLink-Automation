using NUnit.Framework;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;


namespace CentroLink_Automation
{
    public class EditMachineTests : BaseTest
    {

        private LoginPage loginPage;
        private MachineSetupPage machineSetup;
        private EditMachinePage editMachine;


        [SetUp]
        public void Setup()
        {
            loginPage = new LoginPage(driver);
            machineSetup = new MachineSetupPage(driver);
            editMachine = new EditMachinePage(driver);
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

            await LotteryRetailDatabase.ResetTestMachine();
            await LotteryRetailDatabase.DeleteMachine(TestData.TestMachineNumber);
        }


        [Test]
        public void EditMachine_GoTo_Page()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            string machNo = editMachine.GetMachineNumber();
            Assert.AreEqual(TestData.DefaultMachineNumber, machNo);
        }


        [Test]
        public void EditMachine_Prepopulated_Fields()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            string machNo = editMachine.GetMachineNumber();
            Assert.AreEqual(TestData.DefaultMachineNumber,machNo);

            string locMachNo = editMachine.GetLocationMachineNumber();
            Assert.AreEqual(TestData.DefaultLocationMachineNumber,locMachNo);

            string sn = editMachine.GetSerialNumber();
            Assert.AreEqual(TestData.DefaultSerialNumber,sn);

            string ipAddress = editMachine.GetIPAddress();
            Assert.AreEqual(TestData.DefaultIPAddress,ipAddress);
        }


        [Test]
        public void EditMachine_Success()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            int machineRow = machineSetup.RowNumberOfMachine(TestData.DefaultMachineNumber);
            Assert.AreNotEqual(-1, machineRow);

            var machineBefore = machineSetup.GetMachineAtRow(machineRow);

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            editMachine.EnterForm(
                 machineBefore.MachineNumber,
                 machineBefore.LocationMachineNumber,
                 "new SN",
                 machineBefore.IPAddress,
                 0,
                 0
            );

            editMachine.Save();

            Assert.True(editMachine.SuccessWindow.IsOpen);
            editMachine.SuccessWindow.Confirm();

            machineRow = machineSetup.RowNumberOfMachine(TestData.DefaultMachineNumber);
            Assert.AreNotEqual(-1, machineRow);

            var machineAfter = machineSetup.GetMachineAtRow(machineRow);
            Assert.AreNotEqual(machineBefore.SerialNumber,machineAfter.SerialNumber);
        }


        [Test]
        public void EditMachine_ValidationErrors()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            int machineRow = machineSetup.RowNumberOfMachine(TestData.DefaultMachineNumber);
            Assert.AreNotEqual(-1, machineRow);

            var machineBefore = machineSetup.GetMachineAtRow(machineRow);

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            editMachine.EnterIPAddress("invalid");
            editMachine.Save();

            Assert.True(editMachine.IPAddressErrorIsDisplayed());
            editMachine.ReturnToMachineSetup();

            machineRow = machineSetup.RowNumberOfMachine(TestData.DefaultMachineNumber);
            Assert.AreNotEqual(-1, machineRow);

            var machineAfter = machineSetup.GetMachineAtRow(machineRow);
            Assert.AreEqual(machineBefore.IPAddress, machineAfter.IPAddress);
        }


        //Confirmation dialog is displayed when save is successful
        [Test]
        public void EditMachine_Confirmation_Dialog()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            editMachine.EnterSerialNumber("new SN");
            editMachine.Save();

            Assert.True(editMachine.SuccessWindow.IsOpen);
            editMachine.SuccessWindow.Confirm();

            //user is taken back to machine setup
            Assert.Greater(machineSetup.RowCount, 0);
        }


        //If user presses back button, all changes are lost and they are returned to the machine setup
        [Test]
        public void EditMachine_BackButton_Unsaved()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            int machineRow = machineSetup.RowNumberOfMachine(TestData.DefaultMachineNumber);
            Assert.AreNotEqual(-1, machineRow);

            var machineBefore = machineSetup.GetMachineAtRow(machineRow);

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            editMachine.EnterSerialNumber("new SN");

            editMachine.ClickBackButton();
            editMachine.ConfirmationWindow.Confirm();

            machineRow = machineSetup.RowNumberOfMachine(TestData.DefaultMachineNumber);
            Assert.AreNotEqual(-1, machineRow);

            var machineAfter = machineSetup.GetMachineAtRow(machineRow);
            Assert.AreEqual(machineBefore.SerialNumber, machineAfter.SerialNumber);
        }



        [Test]
        public void EditMachine_BackButton_Cancel()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            int machineRow = machineSetup.RowNumberOfMachine(TestData.DefaultMachineNumber);
            Assert.AreNotEqual(-1, machineRow);

            var machineBefore = machineSetup.GetMachineAtRow(machineRow);

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            editMachine.EnterSerialNumber("new SN");

            editMachine.ClickBackButton();
            editMachine.ConfirmationWindow.Cancel();

            editMachine.Save();
            editMachine.SuccessWindow.Confirm();

            machineRow = machineSetup.RowNumberOfMachine(TestData.DefaultMachineNumber);
            Assert.AreNotEqual(-1, machineRow);

            var machineAfter = machineSetup.GetMachineAtRow(machineRow);
            Assert.AreNotEqual(machineBefore.SerialNumber, machineAfter.SerialNumber);
        }


        [Test]
        public async Task EditMachine_AddGameButton_MultiGame_Disabled()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber,false);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            Assert.False(editMachine.AddGameIsEnabled());
        }


        [Test]
        public async Task EditMachine_AddGameButton_MultiGame_Enabled()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            Assert.True(editMachine.AddGameIsEnabled());
        }


        //Game 'Remove' button is disabled in multi-game is disabled
        [Test]
        public async Task EditMachine_RemoveButton_MultiGame_Enabled()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            Assert.True(editMachine.RemoveGameIsEnabled());

            editMachine.RemoveGameByRow(0);
            Assert.AreEqual(0, editMachine.RowCount);
        }


        //Game 'Remove' button is disabled in multi-game is disabled
        [Test]
        public async Task EditMachine_RemoveButton_MultiGame_Disabled()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber, false);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            Assert.False(editMachine.RemoveGameIsEnabled());
        }


        //Verify at least one game has to be selected
        [Test]
        public async Task EditMachine_No_Games_Selected()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            if(editMachine.RowCount < 1)
            {
                editMachine.ClickAddGame();
                editMachine.SelectBank(0, 0);
                editMachine.SelectGame(0, 0);
            }

            editMachine.RemoveGameByRow(0);
            editMachine.Save();

            Assert.False(editMachine.SuccessWindow.IsOpen);
        }


        [Test]
        public async Task EditMachine_GetMachineGames()
        {
            var games = await LotteryRetailDatabase.GetGamesAssignedToMachine(TestData.DefaultMachineNumber);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            string selectedGame = editMachine.GetSelectedGame(0);
            string expected =  $"{games[0].GameCode} - {games[0].GameDescription}";

            Assert.AreEqual(games.Count,editMachine.RowCount);
            Assert.AreEqual(expected,selectedGame);
        }


        [Test]
        public async Task EditMachine_AssignNewGame()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            var games = editMachine.GetGameOptions();
            string currentSelection = editMachine.GetSelectedGame(0);
            var newGame = games.Where(game => game.ToLower().Contains(TestData.TestGameCode.ToLower()) == false).FirstOrDefault();

            editMachine.ClickAddGame();
            editMachine.SelectBank(1, 0);
            editMachine.SelectGame(1, newGame);

            editMachine.Save();
            editMachine.SuccessWindow.Confirm();

            var assignedGames = await LotteryRetailDatabase.GetGamesAssignedToMachine(TestData.DefaultMachineNumber);
            Assert.AreEqual(2,assignedGames.Count);
        }


        [Test]
        public async Task EditMachine_AddGame()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            int gameCountBefore = editMachine.RowCount;

            var games = editMachine.GetGameOptions();
            string currentSelection = editMachine.GetSelectedGame(0);
            var newGame = games.Where(game => game.ToLower().Contains(TestData.TestGameCode.ToLower()) == false).FirstOrDefault();

            editMachine.ClickAddGame();
            editMachine.SelectBank(1, 0);
            editMachine.SelectGame(1, newGame);

            editMachine.Save();
            editMachine.SuccessWindow.Confirm();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            Assert.AreEqual(gameCountBefore + 1, editMachine.RowCount);
        }


        [Test]
        public async Task EditMachine_Change_Assigned_Game()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            string currentBank = editMachine.GetSelectedBank();
            string currentGame = editMachine.GetSelectedGame();

            //find game not already assigned
            var games = editMachine.GetGameOptions();
            var newGame = games.Where(game => game != currentGame).FirstOrDefault();

            editMachine.SelectGame(newGame);
            editMachine.Save();
            editMachine.SuccessWindow.Confirm();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            string currentGameAfter = editMachine.GetSelectedGame();

            Assert.AreNotEqual(currentGame, currentGameAfter);
            Assert.AreEqual(newGame, currentGameAfter);

        }


        [Test]
        public async Task EditMachine_DuplicateGame_Error()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            Assert.Greater(editMachine.RowCount, 0);

            string currentBank = editMachine.GetSelectedBank();
            string currentGame = editMachine.GetSelectedGame();

            editMachine.ClickAddGame();

            editMachine.SelectBank(1,currentBank);
            editMachine.SelectGame(1,currentGame);

            editMachine.Save();

            Assert.False(editMachine.SuccessWindow.IsOpen);
        }


        [Test]
        public async Task EditMachine_RemoveGame()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            int gameCountBefore = editMachine.RowCount;

            var games = editMachine.GetGameOptions();
            string currentSelection = editMachine.GetSelectedGame(0);
            var newGame = games.Where(game => game.ToLower().Contains(TestData.TestGameCode.ToLower()) == false).FirstOrDefault();

            editMachine.ClickAddGame();
            editMachine.SelectBank(1, 0);
            editMachine.SelectGame(1, newGame);

            editMachine.Save();
            editMachine.SuccessWindow.Confirm();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            Assert.AreEqual(gameCountBefore + 1, editMachine.RowCount);

            editMachine.RemoveGameByRow(1);

            Assert.AreEqual(gameCountBefore, editMachine.RowCount);
        }


        [Test]
        public async Task EditMachine_DisableGame()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            //find a game thats not already selected
            var games = editMachine.GetGameOptions();
            string currentSelection = editMachine.GetSelectedGame(0);
            var newGame = games.Where(game => game.ToLower().Contains(TestData.TestGameCode.ToLower()) == false).FirstOrDefault();
            
            //select new game
            editMachine.ClickAddGame();
            editMachine.SelectBank(1, 0);
            editMachine.SelectGame(1, newGame);

            editMachine.Save();
            editMachine.SuccessWindow.Confirm();

            //verify 2 machines are assigned
            var assignedGames = await LotteryRetailDatabase.GetGamesAssignedToMachine(TestData.DefaultMachineNumber);
            Assert.AreEqual(2, assignedGames.Count);

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            //disable the 2nd game
            editMachine.SetGameEnabledByRow(1,false);
            editMachine.Save();
            editMachine.SuccessWindow.Confirm();

            //verify only 1 game is assigned
            assignedGames = await LotteryRetailDatabase.GetGamesAssignedToMachine(TestData.DefaultMachineNumber);
            Assert.AreEqual(1, assignedGames.Count);
        }


        [Test]
        public async Task EditMachine_UnassignGame()
        {
            await LotteryRetailDatabase.UpdateMachineMultiGameEnabled(TestData.DefaultMachineNumber, true);

            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            //find a game thats not already selected
            var games = editMachine.GetGameOptions();
            string currentSelection = editMachine.GetSelectedGame(0);
            var newGame = games.Where(game => game.ToLower().Contains(TestData.TestGameCode.ToLower()) == false).FirstOrDefault();

            //select new game
            editMachine.ClickAddGame();
            editMachine.SelectBank(1, 0);
            editMachine.SelectGame(1, newGame);

            editMachine.Save();
            editMachine.SuccessWindow.Confirm();

            //verify 2 machines are assigned
            var assignedGames = await LotteryRetailDatabase.GetGamesAssignedToMachine(TestData.DefaultMachineNumber);
            Assert.AreEqual(2, assignedGames.Count);

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            //remove the 2nd game
            editMachine.RemoveGameByRow(1);
            editMachine.Save();
            editMachine.SuccessWindow.Confirm();

            //verify only 1 game is assigned
            assignedGames = await LotteryRetailDatabase.GetGamesAssignedToMachine(TestData.DefaultMachineNumber);
            Assert.AreEqual(1, assignedGames.Count);
        }


        [Test]
        public void EditMachine_SetRemoved()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickMachineSetupTab();

            Assert.False(machineSetup.MachineIsRemoved(TestData.DefaultMachineNumber));

            machineSetup.SelectRowByMachineNumber(TestData.DefaultMachineNumber);
            machineSetup.ClickEditMachine();

            editMachine.CheckRemoved();
            editMachine.Save();
            editMachine.SuccessWindow.Confirm();

            Assert.False(machineSetup.MachineFoundInList(TestData.DefaultMachineNumber));

            machineSetup.ShowRemovedMachines();

            Assert.True(machineSetup.MachineFoundInList(TestData.DefaultMachineNumber));
        }
    }
}
