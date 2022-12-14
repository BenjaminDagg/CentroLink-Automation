using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroLink_Automation
{
    public class PromoEntrySchedule
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Started { get; set; }
        public bool Ended { get; set; }
        public int PromoTicketCount { get; set; }
        public int PromoFactorCount { get; set; }


        public static bool AreEqual(PromoEntrySchedule a, PromoEntrySchedule b)
        {
            return a.Description == b.Description &&
                    a.StartTime == b.StartTime &&
                    a.EndTime == b.EndTime;
        }
    }
}
