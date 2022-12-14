using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroLink_Automation
{
    public static class TestData
    {
        /* Machine Setup */
        public const string DefaultMachineNumber = "00001";    //default machine in the list
        public const string DefaultLocationMachineNumber = "00001";
        public const string DefaultIPAddress = "10.0.12.30";
        public const string DefaultSerialNumber = "00001";
        //Machine to test Add,edit,duplicate,delete etc on. Deleted after tests run
        public const string TestMachineNumber = "00002";
        public const string TestLocationMachineNumber = "2";
        public const string TestMachineSerialNumber = "00002";
        public const string TestMachineIpAddress = "54.72.125.232";

        public const int TestBankNumber = 3;
        public const string TestGameCode = "9Z2";

        /* Location Setup */
        public const int LocationId = 339577;
        public const string LocationDgeId = "MO1005";
        public const string LocationName = "American Eagle2";
        public const int LocationRetailerNumber = 4064;
        public const int LocationNameMaxCharacters = 48;
        public const int LocationIdMinRange = 000000;
        public const int LocationIdMaxRange = 999999;
        //location to test add/edit location on. Deleted after each test
        public static int TestLocationId = 339578;
        public const string TestLocationName = "American Eagle3";
        public const string TestLocationDgeId = "MO1006";

        public const string AdminUsername = "user1";
        public const string AdminPassword = "Diamond1#";

        /* Deal Setup */
        public const int TestDealNumber = 120381;   //Works: 120381,120449 (Barcode type 14)

        /* Promo Schedule */
        public const int TestPromoEntryScheduleId = 21;

        /* Transaction Portal */
        public const string TransactionPortalIpAddress = "10.0.50.186";
        public const int TpPort = 4550;
    }
}
