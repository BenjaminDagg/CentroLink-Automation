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
        public void Setup()
        {
            loginPage = new LoginPage(driver);
            machineSetup = new MachineSetupPage(driver);
            addMachinePage = new MachineDetailsPage(driver);
        }


        [Test]
        public async Task Show_Hidden_Machines()
        {

            loginPage.Login("user1", "Diamond1#");
            navMenu.ClickMachineSetupTab();

            //machineSetup.ClickAddMachine();
            machineSetup.ClickAddMachine();

            /*
            addMachinePage.EnterMachineNumber("mNum");
            addMachinePage.EnterLocationMachineNumber("lmn");
            addMachinePage.EnterSerialNumber("SN");
            addMachinePage.EnterIPAddress("ip");
            addMachinePage.EnterDescription("Description");*/

            var banks = addMachinePage.GetBankOptions();
            Console.WriteLine("test");
            addMachinePage.SelectBank(banks[5]);
            Console.WriteLine("You selected: " + addMachinePage.GetSelectedBank());
        }


        
    }
}
