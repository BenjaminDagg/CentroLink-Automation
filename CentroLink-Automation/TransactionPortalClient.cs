using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace CentroLink_Automation
{
    public class TransactionPortalClient
    {
        private TcpClient tcpClient;
        private string hostname;
        private int port;
        public int SequenceNumber { get; set; } = 0;

        public TransactionPortalClient(string ipAddress, int _port)
        {
            hostname = ipAddress;
            port = _port;
            tcpClient = new TcpClient();

        
            
        }


        public void Connect()
        {
            try
            {
                tcpClient.Connect(hostname, port);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public string SendMessage(string message)
        {

            SequenceNumber++;

            if(tcpClient.Connected == false)
            {
                return string.Empty;
            }

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            NetworkStream stream = tcpClient.GetStream();
            StreamWriter writer = new StreamWriter(stream);

            // Send the message to the connected TcpServer.
            writer.WriteLine(message);
            writer.Flush();

            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            StreamReader reader = new StreamReader(stream);
            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            string res = responseData;
            stream.Close();

            return res;

        }


        public void Close()
        {
            SequenceNumber = 0;
            if(tcpClient.Connected && tcpClient.GetStream() != null)
            {
                tcpClient.Client.Disconnect(true);
            }
        }


    }
}
