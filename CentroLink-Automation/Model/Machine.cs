using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroLink_Automation
{
    public class Machine
    {
        public string MachineNumber { get; set; }
        public string LocationMachineNumber { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }    //true = online, false = offline
        public string IPAddress { get; set; }
        public bool Removed { get; set; }
        public string SerialNumber { get; set; }
        public string OperatingSystemVersion { get; set; }
        public double Balance { get; set; }
        public double PromoBalance { get; set; }
        public DateTime LastPlay { get; set; }


        public void Display()
        {
            Console.WriteLine("Machine No: " + MachineNumber);
            Console.WriteLine(" Location Machine No: " + LocationMachineNumber);
            Console.WriteLine("Descriptoin: " + Description);
            Console.WriteLine("Status: " + Status);
            Console.WriteLine("IP Address: " + IPAddress);
            Console.WriteLine("Is Removed: " + Removed);
            Console.WriteLine("Serial No: " + SerialNumber);
            Console.WriteLine("OS Version: " + OperatingSystemVersion);
            Console.WriteLine("Baance: " + Balance);
            Console.WriteLine("Promo balance: " + PromoBalance);
            Console.WriteLine("LastPlay " + LastPlay);
        }
    }
}
