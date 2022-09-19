using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EGMSimulator.Core.Models.EgmGame;
using Framework.Core.Logging;
using EGMSimulator.Core.Services.Implementations;

namespace CentroLink_Automation
{
    public class DealManagerService
    {
        public Deal Deal { get; set; }
        private SqlConnection _connection;
        private BarcodeService BarcodeService;
        public int NextTicket
        {
            get
            {
                return Deal.NextTicket;
            }
            set
            {
                Deal.NextTicket = value;
            }
        }
       
        private SqlConnection DbConnection;

        public async static Task<DealManagerService> BuildDealManagerAsync(int dealNo, SqlConnection conn, ILogService _logService)
        {
            var deal = await GetDealData(dealNo,conn);
            return new DealManagerService(deal,conn,_logService);
        }

        private DealManagerService(Deal deal, SqlConnection conn, ILogService _logService)
        {
            this.Deal = deal;
            this.DbConnection = conn;
            this.BarcodeService = new BarcodeService(_logService);
        }



        private static async Task<Deal> GetDealData(int dealNo,SqlConnection conn)
        {
            var query = "select " +
                            "DS.DEAL_NO, DS.IS_OPEN, DS.CLOSE_RECOMMENDED, DS.DEAL_DESCR, " +
                            "DS.TAB_AMT, DSTATS.PLAY_COUNT, DS.TABS_PER_ROLL, DS.NUMB_ROLLS, " +
                            "DS.GAME_CODE, DSEQ.NEXT_TICKET,DSEQ.LAST_TICKET,DSEQ.DENOMINATION, " +
                            "DSEQ.COINS_BET,DSEQ.LINES_BET " +
                        "from DEAL_SETUP as DS " +
                        "inner join DEAL_STATS DSTATS " +
                        "on DS.DEAL_NO = DSTATS.DEAL_NO " +
                        "inner join DEAL_SEQUENCE DSEQ " +
                        "on DSEQ.DEAL_NO = DS.DEAL_NO " +
                        "where DS.DEAL_NO = @DealNo";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.Add("@DealNo", System.Data.SqlDbType.Int).Value = dealNo;

            var reader = await command.ExecuteReaderAsync();

            Deal deal = new Deal();

            while (reader.Read())
            {
                deal.DealNumber = reader.GetInt32(0);
                deal.IsOpen = reader.GetBoolean(1);
                deal.RecommendedToClose = reader.GetBoolean(2);
                deal.Description = reader.GetString(3);
                deal.TabAmount = (double)reader.GetDecimal(4);
                deal.TabsPlayed = reader.GetInt32(5);
                deal.TabsPerRoll = reader.GetInt32(6);
                deal.NumRolls = reader.GetInt32(7);
                deal.GameCode = reader.GetString(8);
                deal.NextTicket = reader.GetInt32(9);
                deal.LastTicket = reader.GetInt32(10);
                deal.Denomination = reader.GetInt32(11);
                deal.CoinsBet = reader.GetInt16(12);
                deal.LinesBet = reader.GetByte(13);
            }
            
            return deal;
        }


        public async Task<Deal> RetrievDeal()
        {
            var query = "select " +
                            "DS.DEAL_NO, DS.IS_OPEN, DS.CLOSE_RECOMMENDED, DS.DEAL_DESCR, " +
                            "DS.TAB_AMT, DSTATS.PLAY_COUNT, DS.TABS_PER_ROLL, DS.NUMB_ROLLS, " +
                            "DS.GAME_CODE, DSEQ.NEXT_TICKET,DSEQ.LAST_TICKET,DSEQ.DENOMINATION, " +
                            "DSEQ.COINS_BET,DSEQ.LINES_BET " +
                        "from DEAL_SETUP as DS " +
                        "inner join DEAL_STATS DSTATS " +
                        "on DS.DEAL_NO = DSTATS.DEAL_NO " +
                        "inner join DEAL_SEQUENCE DSEQ " +
                        "on DSEQ.DEAL_NO = DS.DEAL_NO " +
                        "where DS.DEAL_NO = @DealNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@DealNo", System.Data.SqlDbType.Int).Value = Deal.DealNumber;

            var reader = await command.ExecuteReaderAsync();

            Deal deal = new Deal();

            while (reader.Read())
            {
                deal.DealNumber = reader.GetInt32(0);
                deal.IsOpen = reader.GetBoolean(1);
                deal.RecommendedToClose = reader.GetBoolean(2);
                deal.Description = reader.GetString(3);
                deal.TabAmount = (double)reader.GetDecimal(4);
                deal.TabsPlayed = reader.GetInt32(5);
                deal.TabsPerRoll = reader.GetInt32(6);
                deal.NumRolls = reader.GetInt32(7);
                deal.GameCode = reader.GetString(8);
                deal.NextTicket = reader.GetInt32(9);
                deal.LastTicket = reader.GetInt32(10);
                deal.Denomination = reader.GetInt32(11);
                deal.CoinsBet = reader.GetInt16(12);
                deal.LinesBet = reader.GetByte(13);
            }

            this.Deal = deal;

            return deal;
        }


        public int NextWinningTicket(int startTicket)
        {
            var storedProc = "create PROCEDURE ReadDealBarocdesDynamic(@TableName, NVARCHAR(50), @StartTicket Int)" +
                                " as " +
                                "BEGIN " +
                                "DECLARE @SQLSelectQuery NVARCHAR(MAX)='' " +
                                "SET @SQLSelectQUERY = 'select TicketNumber, Barcode from ' + @TableName + ' where TicketNumber > @StartTicket' " +
                                "exec(@SqlSelectQuery) " +
                              "end";

            var command = new SqlCommand("ReadDealBarocdesDynamic", DbConnection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@TableName", System.Data.SqlDbType.VarChar).Value = $"Deal{Deal.DealNumber}";
            command.Parameters.Add("@StartTicket", System.Data.SqlDbType.Int).Value = startTicket;

            var reader = command.ExecuteReader();

                
            while (reader.Read())
            {
                int ticketNum = reader.GetInt32(0);
                string barcode = reader.GetString(1);

                EGMSimulator.Core.Models.EgmGame.Barcode encryptedBarcode = new EGMSimulator.Core.Models.EgmGame.Barcode { EncryptedBarcode = barcode };

                var decryptedBarcode = BarcodeService.DecryptBarcode(encryptedBarcode);

                if(decryptedBarcode.DecryptedCreditsWon > 0)
                {
                    this.NextTicket = ticketNum;
                    return ticketNum;
                }
            }

            return 0;
        }
    }
}
