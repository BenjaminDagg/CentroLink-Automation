using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroLink_Automation
{
    public class Deal
    {
        public int DealNumber { get; set; }
        public bool IsOpen { get; set; }    //Status
        public bool RecommendedToClose { get; set; }
        public string Description { get; set; }
        public double TabAmount { get; set; }
        public int TabsDispensed { get; set; }
        public int TabsPerDeal { get; set; }
        public double Completed { get; set; }
        public DateTime DealOpen { get; set; }
        public int NumRolls { get; set; }
        public string FormNumber { get; set; }
        public char DealType { get; set; }
        public string GameCode { get; set; }
        public int TabsPlayed { get; set; }
        public int ProductId { get; set; }
        public double CostPerRoll { get; set; }
        public double JackbotBase { get; set; }
        public int NextTicket { get; set; }
        public int LastTicket { get; set; }
        public int Denomination { get; set; }
        public int CoinsBet { get; set; }
        public int LinesBet { get; set; }
        public int TabsPerRoll { get; set; }
        // select DS.DEAL_NO, DS.IS_OPEN, DS.CLOSE_RECOMMENDED, DS.DEAL_DESCR, DS.TAB_AMT, DSTATS.PLAY_COUNT, DS.TABS_PER_ROLL, DS.NUMB_ROLLS, DS.GAME_CODE, 

        public void Display()
        {
            Console.WriteLine("Deal No " + DealNumber);
            Console.WriteLine("Status  " + IsOpen);
            Console.WriteLine("Recc CLose " + RecommendedToClose);
            Console.WriteLine("Desc " + Description);
            Console.WriteLine("Tab Amount " + TabAmount);
            Console.WriteLine("Tabs Dispensed " + TabsDispensed);
            Console.WriteLine("Tabs Per Deal " + TabsPerDeal);
            Console.WriteLine("Completed %" + Completed);
            Console.WriteLine("Open Date " + DealOpen);
        }
    }
}
