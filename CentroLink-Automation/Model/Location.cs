using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroLink_Automation
{
    public class Location
    {
        public string DgeId { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int RetailerNumber { get; set; }
        public bool IsDefault { get; set; }
        public DateTime AccountDayStart { get; set; }
        public DateTime AccountDayEnd { get; set; }
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int PostalCode { get; set; }
        public string Phone { get; set; }
        public string? Fax { get; set; }
        public string? SweepAmount { get; set; }
        public bool JackpotLockup { get; set; }
        public bool PrintPromoTickets { get; set; }
        public string TPI { get; set; }
        public int CashoutTImeout { get; set; }
        public decimal MaxBalanceAdjustment { get; set; }
        public decimal PayoutAuthorizationAmount { get; set; }
        public bool AllowTicketReprint { get; set; }
        public bool SummarizePlayForHoldByDenomReport { get; set; }
        public bool AutoDropOnCashDoorOpen { get; set; }
        public decimal LockupAmount { get; set; }
    }
}
