using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CentroLink_Automation
{
    public class BankSetupListTests : BaseTest
    {

        private LoginPage loginPage;
        private BankSetupListPage bankList;


        [SetUp]
        public override async Task Setup()
        {
            base.Setup();

            loginPage = new LoginPage(driver);
            bankList = new BankSetupListPage(driver);
        }


        [TearDown]
        public override async Task EndTest()
        {
            base.EndTest();

        }


        [Test]
        public void BankSetup_GoTo_Page()
        {
            loginPage.Login(TestData.AdminUsername, TestData.AdminPassword);
            navMenu.ClickBankSetupTab();

            var bank = bankList.GetBank(777);

            Console.WriteLine("Bank No: " + bank.BankNumber);
            Console.WriteLine("Desc: " + bank.Description);
            Console.WriteLine("GTC: " + bank.GameTypeCode);
            Console.WriteLine("IsPaper: " + bank.IsPaper);
            Console.WriteLine("Lockup AMount: " + bank.LockupAmount);
            Console.WriteLine("DBA Lockup AMount: " + bank.DBALockupAmount);
            Console.WriteLine("Product: " + bank.Product);
            Console.WriteLine("Product Line: " + bank.ProductLine);
            Console.WriteLine("Promo Ticket factor: " + bank.PromoTicketFactor);
            Console.WriteLine("Promo ticket amount: " + bank.PromoTicketAmount);
        }
    }
}
