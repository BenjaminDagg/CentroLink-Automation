using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroLink_Automation
{
    public class Barcode
    {
        public string EncryptedBarcode { get; set; }
        public string DecryptedBarcodeText { get; set; }
        public int DecryptedTicketNumber { get; set; }
        public string DecryptedReelStops { get; set; }
        public int DecryptedTier { get; set; }
        public int DecryptedCreditsWon { get; set; }
        public int DecryptedBonus1 { get; set; }
        public int DecryptedBonus2 { get; set; }
        public int DecryptedWinType { get; set; }
        public int BarcodeTypeID { get; set; }
    }
}
