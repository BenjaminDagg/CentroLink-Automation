using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGMSimulator.Core.Services.Implementations;
using EGMSimulator.Core.Models.EgmGame;
using Framework.Core.Logging;
using EGMSimulator.Core.Models.EgmGame;
using Framework.Core.Logging;

namespace CentroLink_Automation
{
    public class TransactionPortalService
    {
        private TransactionPortalClient tpClient;
        private Machine Machine;
        private DealManagerService DealManager;
        private DatabaseManager DatabaseManager;
        private MachineManagerService MachineManager;
        private int sequenceNumber;
        private BarcodeService BarcodeService;
        private GameManagerService GameService;

        public TransactionPortalService(MachineManagerService MachineService,
            DealManagerService dealManager,
            DatabaseManager database,
            GameManagerService gameManager,
            ILogService logService)
        {
            tpClient = new TransactionPortalClient("10.0.50.186", 4550);
            Machine = MachineService.Machine;
            DealManager = dealManager;
            DatabaseManager = database;
            this.MachineManager = MachineService;
            SequenceNumber = MachineService.Machine.SequenceNumber;
            
            this.GameService = gameManager;
            
           BarcodeService = new BarcodeService(logService);
        }


        public int SequenceNumber
        {
            get { return sequenceNumber; }
            set
            {
                sequenceNumber = value;
                MachineManager.Machine.SequenceNumber = value;
            }
        }


        public int BetAmount
        {
            get
            {
                return (DealManager.Deal.LinesBet * DealManager.Deal.CoinsBet) * DealManager.Deal.Denomination;
            }
        }


        public async Task Connect()
        {
            this.Machine = await MachineManager.RetrieveMachine();
            DealManager.Deal = await DealManager.RetrievDeal();

            tpClient.Connect();
        }


        public void Disconnect()
        {
            tpClient.CLose();
            MachineManager.ResetMachine();
        }


        private string ParseBarcode(string transResponse)
        {
            int commaIndex = transResponse.LastIndexOf(',');

            var barcodeRs = transResponse.Substring(commaIndex + 1).Trim();
            string barcode = barcodeRs;

            return barcode;
        }


        public string PlayLosingGame()
        {
            int ticketNum = DealManager.NextTicket;
            int balanceCredits = (int)Machine.Balance * 100;
            int denom = DealManager.Deal.Denomination;
            int betAmount = (DealManager.Deal.LinesBet * DealManager.Deal.CoinsBet) * denom;
            
            string tTransResponse = TransactionT();

            string ticketBarcodeString = ParseBarcode(tTransResponse);
            
            int newBalance = balanceCredits - betAmount;
            Machine.Balance = newBalance / 100;

            var lTrans = $"{SequenceNumber},L,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{newBalance},0,{0},0,0,{DealManager.Deal.Denomination},{DealManager.Deal.CoinsBet},{DealManager.Deal.LinesBet},{DealManager.Deal.DealNumber},0,{ticketNum},{ticketBarcodeString},0";
           
            var lTransResponse = tpClient.Execute(lTrans);
            
            DealManager.NextTicket++;
            SequenceNumber++;

            return lTransResponse;
        }


        public string PlayGame()
        {
            int ticketNum = DealManager.NextTicket;
            int balanceCredits = (int)Machine.Balance * 100;
            int denom = DealManager.Deal.Denomination;
            int betAmount = (DealManager.Deal.LinesBet * DealManager.Deal.CoinsBet) * denom;
            
            string tTransResponse = TransactionT();
            
            string ticketBarcodeString = ParseBarcode(tTransResponse);
            var barcode = BarcodeService.DecryptBarcode(GameService.Game.BarcodeTypeId, ticketBarcodeString);

            int creditsWon = barcode.DecryptedCreditsWon;

            if(creditsWon > 0)
            {
                return TransW(ticketBarcodeString);
            }
            else
            {
                return TransL(ticketBarcodeString);
            }
        }


        public string PlayWinningGame(out int winAmountCredits)
        {
            
            int ticketNum = DealManager.NextTicket;
            int balanceCredits = (int)Machine.Balance * 100;
            int denom = DealManager.Deal.Denomination;
            int betAmount = (DealManager.Deal.LinesBet * DealManager.Deal.CoinsBet) * denom;

            string tTransResponse = TransactionT();
            string ticketBarcodeString = ParseBarcode(tTransResponse);
            var barcode = BarcodeService.DecryptBarcode(GameService.Game.BarcodeTypeId, ticketBarcodeString);

            while(barcode.DecryptedCreditsWon < 1)
            {
                
                DealManager.NextTicket++;
                sequenceNumber++;
                tTransResponse = TransactionT();
                ticketBarcodeString = ParseBarcode(tTransResponse);
                barcode = BarcodeService.DecryptBarcode(GameService.Game.BarcodeTypeId, ticketBarcodeString);
            }

            winAmountCredits = barcode.DecryptedCreditsWon * DealManager.Deal.CoinsBet;

            return TransW(ticketBarcodeString);
        }


        private string TransL(string encryptedTicketBarcode)
        {
            int ticketNum = DealManager.NextTicket;
            int balanceCredits = (int)Machine.Balance * 100;
            int denom = DealManager.Deal.Denomination;
            int betAmount = (DealManager.Deal.LinesBet * DealManager.Deal.CoinsBet) * denom;

            int newBalance = balanceCredits - betAmount;
            Machine.Balance = newBalance / 100;

            var lTrans = $"{SequenceNumber},L,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{newBalance},0,{0},0,0,{DealManager.Deal.Denomination},{DealManager.Deal.CoinsBet},{DealManager.Deal.LinesBet},{DealManager.Deal.DealNumber},0,{ticketNum},{encryptedTicketBarcode},0";

            var lTransResponse = tpClient.Execute(lTrans);

            DealManager.NextTicket++;
            SequenceNumber++;

            return lTransResponse;
        }


        public string TransW(string encryptedTicketBarcode)
        {

            int ticketNum = DealManager.NextTicket;
            int balanceCredits = (int)Machine.Balance * 100;
            int denom = DealManager.Deal.Denomination;
            int betAmount = (DealManager.Deal.LinesBet * DealManager.Deal.CoinsBet) * denom;
            
            string ticketBarcodeString = ParseBarcode(encryptedTicketBarcode);
            var barcode = BarcodeService.DecryptBarcode(GameService.Game.BarcodeTypeId, ticketBarcodeString);
            int coinsWon = barcode.DecryptedCreditsWon * DealManager.Deal.CoinsBet;

            int tierLevel = barcode.DecryptedTier;
            
            int newBalance = balanceCredits - betAmount;
            newBalance += coinsWon;
            Machine.Balance = newBalance / 100;

            var wTrans = $"{SequenceNumber},W,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{newBalance},0,,{coinsWon},{tierLevel},{DealManager.Deal.Denomination},{DealManager.Deal.CoinsBet},{DealManager.Deal.LinesBet},{DealManager.Deal.DealNumber},0,{ticketNum},{encryptedTicketBarcode},0,0";

            var wTransResponse = tpClient.Execute(wTrans);

            DealManager.NextTicket++;
            SequenceNumber++;

            return wTransResponse;
        }


        public string InsertCash(int dollarAmount)
        {
            int currentBalance = (int)Machine.Balance * 100;
            int denom = DealManager.Deal.Denomination;
            int amountAddedCredits = dollarAmount * 100;
            int newBalance = currentBalance + amountAddedCredits;

            var MTrans = $"{SequenceNumber},M,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{newBalance},,{amountAddedCredits},{denom}";
            var response = tpClient.Execute(MTrans);

            SequenceNumber++;
            Machine.Balance = newBalance / 100;

            return response;
        }


        public string CashOut()
        {
            string createVoucherResponse = VoucherCreate();
            string voucher = ParseVoucherBarcode(createVoucherResponse);

            string printVoucherResponse = VoucherPrint(voucher);

            return printVoucherResponse;
        }


        public string VoucherCreate()
        {

            int currentBalance = (int)Machine.Balance * 100;
            int transAmount = currentBalance;
            int jackpotFlag = 0;
            int sessionAmount = 0;

            var voucherCreateTrans = $"{SequenceNumber},VoucherCreate,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{currentBalance},,{transAmount},{jackpotFlag},{sessionAmount}";
            var response = tpClient.Execute(voucherCreateTrans);

            sequenceNumber++;
            Machine.Balance = 0;

            return response;
        }


        public string VoucherPrint(string voucher)
        {
           
            var voucherCreateTrans = $"{SequenceNumber},VoucherPrinted,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{0},,{voucher},500";
            var response = tpClient.Execute(voucherCreateTrans);

            sequenceNumber++;

            return response;
        }


        public string TransactionT()
        {
            int ticketNum = DealManager.NextTicket;
            int betAmount = DealManager.Deal.LinesBet * DealManager.Deal.CoinsBet;

            var ttrans = $"{SequenceNumber},T,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{DealManager.Deal.Denomination},{DealManager.Deal.DealNumber},{ticketNum},{DealManager.Deal.CoinsBet},{DealManager.Deal.LinesBet}";

            var response = tpClient.Execute(ttrans);
            
            return response;
        }


        public string SetOffline()
        {
            string transX = $"{sequenceNumber},X,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},300";
            var response = tpClient.Execute(transX);

            sequenceNumber++;

            return response;
        }


        public string SetOnline()
        {
            string transX = $"{sequenceNumber},X,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},0";
            var response = tpClient.Execute(transX);

            sequenceNumber++;

            return response;
        }


        private string ParseVoucherBarcode(string voucherText)
        {
            int startIndex = -1;
            int count = 0;
            for (int i = 0; i < voucherText.Length; i++)
            {
                if (voucherText[i] == '|')
                {
                    count++;
                    if (count == 3)
                    {
                        startIndex = i;
                    }
                }
            }
            
            if(startIndex != -1)
            {
                string vouhcer = voucherText.Substring(startIndex + 1, 24);
                vouhcer = vouhcer.Replace("-", "");
                return vouhcer;
            }

            return string.Empty;
        }
    }
}
