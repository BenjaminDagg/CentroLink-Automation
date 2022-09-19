using NUnit.Framework;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using EGMSimulator.Core.Models.EgmGame;
using Framework.Core.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace CentroLink_Automation
{
    public class LoginTests : BaseTest
    {
        private LoginPage loginPage;
        private TransactionPortalClient tpClient;
        private TransactionPortalService TpService;
        private ILogService _logService;

        [SetUp]
        public async Task Setup()
        {
            _logService = ServiceProvider.GetService<ILogService>();

            await LotteryRetailDatabase.ResetTestMachine();

            DealManagerService DealService = await DealManagerService.BuildDealManagerAsync(TestData.TestDealNumber, DbConnection);
            Console.WriteLine("Got play count: " + DealService.Deal.GameCode);

            MachineManagerService MachineService = await MachineManagerService.MachineManagerServiceAsync(int.Parse(TestData.DefaultMachineNumber), DbConnection);
            Console.WriteLine("Got seqs # " + MachineService.Machine.SequenceNumber);

            GameManagerService GameService = await GameManagerService.BuildGameManagerServiceAsync(TestData.TestGameCode, DbConnection);


            TpService = new TransactionPortalService(MachineService,DealService,LotteryRetailDatabase, GameService, _logService);
            TpService.Connect();
        }

        [TearDown]
        public async Task EndTest()
        {
            //tpClient.CLose();
            TpService.Disconnect();
            //await LotteryRetailDatabase.ResetTestMachine();
        }

        [Test]
        public void Succussful_Login()
        {
            loginPage.Login("user1", "Diamond1#");
            
        }


        [Test]
        
        public async Task Test()
        {
            
            string transL = "1000,L,2022-03-02 10:51:47,9900,0,,0,0,1,2,50,68963,0,28830,I5Ke85QGFOAJ5X0GFW2XQVXLE9j3Q4NPF67SSaZZ,0";
            string transA = "A,2022-09-15 19:53:3,0,,0,Status,1.00.02,Devil Sevens";

            //seq#,X,timestamp - error code 0 = startup
            string setOnline = "1,X,2022-09-16 10:26:34,0";

            //error code 300 = TPC shutdown initiated
            string setOffline = $"2,X,2022-09-16 11:12:22,300";

            int balance =300;
            int promoBalance = 0;
            string cardAccount = "";
            int coinsWon = 0;
            int tierLevel = 0;
            int machineDenom = 1;  //10
            int coinsBet = 10;
            int linesBet = 20;
            int dealNo = 120381;
            int rollNo = 0;
            int ticketNum = 25082; //25076
            string barcode = "";
            int pressUpCount = 0;

            //var loss = $"{tpClient.SequenceNumber},L,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{balance},{promoBalance},{cardAccount},{coinsWon},{tierLevel},{machineDenom},{coinsBet},{linesBet},{dealNo},{rollNo},{ticketNum},{barcode},{pressUpCount}";
            var ttrans = $"{10},T,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{machineDenom},{dealNo},{ticketNum},{coinsBet},{linesBet}";



            //var response = tpClient.Execute(ttrans);

            //int commaIndex = response.LastIndexOf(',');

            //var barcodeRs = response.Substring(commaIndex + 1).Trim();
            //barcode = barcodeRs;
            Console.WriteLine("Bbarcode: '" + barcode + "'");
            var loss = $"{11},L,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{balance},{promoBalance},{cardAccount},{coinsWon},{tierLevel},{machineDenom},{coinsBet},{linesBet},{dealNo},{rollNo},{ticketNum},{barcode},{pressUpCount}";
            //var loss = $"{tpClient.SequenceNumber},L,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{balance},0,,0,0,1,10,20,120380,0,25076,645MLak4PWFDKChGF2ihVKHRGaX2bfZZ,0";

            //response = tpClient.Execute(loss);

            //var successfullLoss = "14,L,2022-03-16 08:43:18,300,0,,0,0,1,10,20,120380,0,25077,MhP3feTY1EbcWNEYPSAZk1IfP1jTdJjZ,0";


            //int commaIndex = response.LastIndexOf(',');

            //var barcodeRs = response.Substring(commaIndex + 1).Trim();
            //barcode = barcodeRs;


            //var loss = $"{2},L,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{balance},{promoBalance},{cardAccount},{coinsWon},{tierLevel},{machineDenom},{coinsBet},{linesBet},{dealNo},{rollNo},{ticketNum},{barcode},{pressUpCount}";

            //response = tpClient.SendMessage(loss);
            //Thread.Sleep(15000);

            
            var response = TpService.PlayGame();
            Console.WriteLine(response);
        }

        [Test]
        public async Task Test3()
        {
            TpService.PlayLosingGame();
            TpService.CashOut();
            Thread.Sleep(15000);
        }
    }
}