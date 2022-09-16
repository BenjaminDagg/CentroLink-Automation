using NUnit.Framework;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;

namespace CentroLink_Automation
{
    public class LoginTests 
    {
        private LoginPage loginPage;
        private TransactionPortalClient tpClient;

        [SetUp]
        public void Setup()
        {
            //loginPage = new LoginPage(driver);
            tpClient = new TransactionPortalClient("10.0.50.186",4550);
            tpClient.Connect();
        }

        [TearDown]
        public void EndTest()
        {
            tpClient.Close();
        }

        [Test]
        public void Succussful_Login()
        {
            //loginPage.Login("user1", "Diamond1#");
            
        }


        [Test]
        public async Task Test()
        {
            
            string transL = "1000,L,2022-03-02 10:51:47,9900,0,,0,0,1,2,50,68963,0,28830,I5Ke85QGFOAJ5X0GFW2XQVXLE9j3Q4NPF67SSaZZ,0";
            string transA = "A,2022-09-15 19:53:33,0,,0,Status,1.00.02,Devil Sevens";

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


            
            
            
            
           
            string response = tpClient.SendMessage(transL);
            Console.WriteLine(response);
            Thread.Sleep(10000);
        }

        [Test]
        public void Test2()
        {
            string response = tpClient.SendMessage("1,W,2022-03-02 10:51:47,9900,0,,0,0,1,2,50,68963,0,28830,I5Ke85QGFOAJ5X0GFW2XQVXLE9j3Q4NPF67SSaZZ,0");
            Console.WriteLine(response);
            Thread.Sleep(5000);
        }
    }
}