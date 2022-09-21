using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroLink_Automation
{
    public class Bank
    {
        public int BankNumber { get; set; } 
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string GameTypeCode { get; set; }
        public bool IsPaper { get; set; }
        public double LockupAmount { get; set; }
        public double DBALockupAmount { get; set; }
        public string Product { get; set; }
        public string ProductLine { get; set; }
        public int PromoTicketFactor { get; set; }
        public double PromoTicketAmount { get; set; }
    }
}
