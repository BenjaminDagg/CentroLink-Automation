using NUnit.Framework;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using EGMSimulator.Core.Services.Implementations;
using EGMSimulator.Core.Services;
using Framework.Infrastructure.Modularity;
using System.Collections.Generic;
using EGMSimulator.Core.Exceptions;

namespace CentroLink_Automation
{
    public class LoginTests : BaseTest
    {
        private LoginPage loginPage;
        private TransactionPortalClient tpClient;

        [SetUp]
        public async Task Setup()
        {
            //base.Setup();

            await LotteryRetailDatabase.ResetTestMachine();

            //loginPage = new LoginPage(driver);
            tpClient = new TransactionPortalClient("10.0.50.186",4550);
            tpClient.Connect();

            
        }

        [TearDown]
        public void EndTest()
        {
            
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
            string setOnline = "81,X,2022-09-16 10:26:34,0";

            //error code 300 = TPC shutdown initiated
            string setOffline = $"147,X,2022-09-16 11:12:22,300";

            /*
            Int32 port = 4550;
            TcpClient client = new TcpClient("10.0.50.186", port);

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(transL);
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine(transL);
            writer.Flush();

            Console.WriteLine("Sent: {0}", transL);

            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);
            Thread.Sleep(15000);
            // Close everything.
            stream.Close();
            client.Close();
            */

            //var shutDownRequest = $"{tpClient.SequenceNumber},X,"

            int balance = 90;
            int promoBalance = 0;
            string cardAccount = "";
            int coinsWon = 0;
            int tierLevel = 0;
            int machineDenom = 10;  //10
            int coinsBet = 1;
            int linesBet = 10;
            int dealNo = 120207;
            int rollNo = 0;
            int ticketNum = 14;
            string barcode = "KFfYMT63eO9RFCIde8RaYL856YM74K5Z";
            int pressUpCount = 0;

            //var loss = $"{tpClient.SequenceNumber},L,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{balance},{promoBalance},{cardAccount},{coinsWon},{tierLevel},{machineDenom},{coinsBet},{linesBet},{dealNo},{rollNo},{ticketNum},{barcode},{pressUpCount}";
            var ttrans = $"{tpClient.SequenceNumber},T,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{machineDenom},{dealNo},{ticketNum},{coinsBet},{linesBet}";
            


            var response = tpClient.SendMessage(ttrans);
            Console.WriteLine(response);
            int commaIndex = response.LastIndexOf(',');

            var barcodeRs = response.Substring(commaIndex + 1).Trim();
            barcode = barcodeRs;
            

            var loss = $"{tpClient.SequenceNumber},L,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{balance},{promoBalance},{cardAccount},{coinsWon},{tierLevel},{machineDenom},{coinsBet},{linesBet},{dealNo},{rollNo},{ticketNum},{barcode},{pressUpCount}";

            response = tpClient.SendMessage(loss);

            tpClient.Close();
        }
    }
}