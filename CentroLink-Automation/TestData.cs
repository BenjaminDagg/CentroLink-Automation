using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroLink_Automation
{
    public static class TestData
    {
        public const string DefaultMachineNumber = "00001";    //default machine in the list
        public const string DefaultLocationMachineNumber = "00001";
        public const string DefaultIPAddress = "111.111.111.111";
        public const string DefaultSerialNumber = "00001";
        //Machine to test Add,edit,duplicate,delete etc on. Deleted after tests run
        public const string TestMachineNumber = "00002";
        public const string TestLocationMachineNumber = "2";
        public const string TestMachineSerialNumber = "00002";
        public const string TestMachineIpAddress = "54.72.125.232";

        public const int LocationId = 339577;

        public const string AdminUsername = "user1";
        public const string AdminPassword = "Diamond1#";
    }
}
