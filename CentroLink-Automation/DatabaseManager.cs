using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Transactions;
using System.Globalization;

namespace CentroLink_Automation
{
    public class DatabaseManager
    {
        private SqlConnection DbConnection;

        public DatabaseManager(SqlConnection conn)
        {
            this.DbConnection = conn;
        }


        public void GetMachines()
        {
            Console.WriteLine("in");
            var query = "select * from MACH_SETUP";

            SqlCommand command = new SqlCommand(query, DbConnection);
            

            var reader =  command.ExecuteReader();

            while (reader.Read())
            {
                var machNum = reader.GetString(0);
                Console.WriteLine(machNum);
            }
        }


        //Resets test machine data back to original state after a test
        public async Task ResetTestMachine()
        {
            //delete games other than default from MACH_SETUP_GAMES
            var removeGamesQuery = "delete from MACH_SETUP_GAMES where MACH_NO = @MachNo and GAME_CODE <> @GameCode";

            //set MACH_SETUP fields back to default
            var restMachineQuery = "update MACH_SETUP " +
                            "set " +
                            "REMOVED_FLAG = 0, " +
                            "ACTIVE_FLAG = 1, " +
                            "CASINO_MACH_NO = @LocationMachineNumber ," +
                            "MultiGameEnabled = 0, " +
                            "MACH_SERIAL_NO = @SerialNumber, " +
                            "IP_ADDRESS = @IPAddress, " +
                            "BANK_NO = @BankId, " +
                            "GAME_CODE = @GameCode ," +
                            "Balance = 5.00" +
                        "where MACH_NO = @MachNo";

            //Assign default game to machine if not already assigned
            var assignDefaultGameQuery = "begin " +
                                            "if not exists " +
                                                "(select * from MACH_SETUP_GAMES " +
                                                "where GAME_CODE = @GameCode " +
                                                "and MACH_NO = @MachNo) " +
                                            "begin " +
                                                "insert into MACH_SETUP_GAMES " +
                                                "([MACH_NO],[LOCATION_ID],[GAME_CODE],[BANK_NO],[TYPE_ID]," +
                                                "[GAME_RELEASE],[MATH_DLL_VERSION],[MATH_LIB_VERSION],[IsEnabled]) " +
                                                "values" +
                                                "(@MachNo,@LocationId,@GameCode,@BankId,'S',NULL,NULL,NULL,1) " +
                                            "end " +
                                         "end";
            

            SqlCommand command = new SqlCommand(restMachineQuery, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = TestData.DefaultMachineNumber;
            command.Parameters.Add("@LocationMachineNumber", System.Data.SqlDbType.VarChar).Value = TestData.DefaultLocationMachineNumber;
            command.Parameters.Add("@SerialNumber", System.Data.SqlDbType.VarChar).Value = TestData.DefaultSerialNumber;
            command.Parameters.Add("@IPAddress", System.Data.SqlDbType.VarChar).Value = TestData.DefaultIPAddress;
            command.Parameters.Add("@BankId", System.Data.SqlDbType.VarChar).Value = TestData.TestBankNumber;
            command.Parameters.Add("@GameCode", System.Data.SqlDbType.VarChar).Value = TestData.TestGameCode;
            command.Parameters.Add("@LocationId", System.Data.SqlDbType.VarChar).Value = TestData.LocationId;

            //execute commands
            var result = await command.ExecuteNonQueryAsync();

            command.CommandText = removeGamesQuery;
            result = await command.ExecuteNonQueryAsync();

            command.CommandText = assignDefaultGameQuery;
            result = await command.ExecuteNonQueryAsync();

            await UpdateMachineLastPlay(TestData.DefaultMachineNumber);
        }


        public async Task ResetTestBank()
        {

            var query = "update BANK set " +
                            "BANK_DESCR = 'AD BnB PullTab 1 Dollar'," +
                            "PROG_FLAG = 0, " +
                            "PROG_AMT = '0.00'," +
                            "LAST_JP_AMOUNT = NULL, " +
                            "LAST_JP_TIME = NULL, " +
                            "GAME_TYPE_CODE = '9X'," +
                            "PRODUCT_LINE_ID = 18," +
                            "IS_PAPER = 1, " +
                            "LOCKUP_AMOUNT = 600.01," +
                            "ENTRY_TICKET_FACTOR = 3, " +
                            "ENTRY_TICKET_AMOUNT = 0.00, " +
                            "DBA_LOCKUP_AMOUNT = 500.00, " +
                            "IS_ACTIVE = 1 " +
                         "where BANK_NO = @BankNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@BankNo", System.Data.SqlDbType.VarChar).Value = TestData.TestBankNumber;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task UpdateMachineRemovedFlag(string machNo, bool isRemoved)
        {

            var query = "update MACH_SETUP set REMOVED_FLAG = @IsActive where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = TestData.DefaultMachineNumber;
            command.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = isRemoved;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task UpdateMachineActivedFlag(string machNo, bool isActive)
        {

            var query = "update MACH_SETUP set ACTIVE_FLAG = @IsActive where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = TestData.DefaultMachineNumber;
            command.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = isActive;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task AddMachine(string machNo, string locMachineNum,string sn, string ipAddress, string Description)
        {
            var query = "BEGIN " +
                            "if not exists (select * from MACH_SETUP " +
                                "where MACH_NO = @MachNo) " +
                            "BEGIN " +
                                "INSERT INTO [dbo].[MACH_SETUP] (" +
                                    "[MACH_NO] ," +
                                    "[LOCATION_ID] ," +
                                    "[MODEL_DESC] ," +
                                    "[TYPE_ID] ," +
                                    "[GAME_CODE] ," +
                                    "[BANK_NO] ," +
                                    "[CASINO_MACH_NO] ," +
                                    "[IP_ADDRESS] ," +
                                    "[PLAY_STATUS] ," +
                                    "[PLAY_STATUS_CHANGED] ," +
                                    "[ACTIVE_FLAG] ," +
                                    "[REMOVED_FLAG] ," +
                                    "[BALANCE] ," +
                                    "[PROMO_BALANCE] ," +
                                    "[LAST_ACTIVITY] ," +
                                    "[LAST_CONNECT] ," +
                                    "[LAST_DISCONNECT] ," +
                                    "[MACH_SERIAL_NO] ," +
                                    "[VOUCHER_PRINTING] ," +
                                    "[METER_TRANS_NO] ," +
                                    "[SD_RS_FLAG] ," +
                                    "[GAME_RELEASE] ," +
                                    "[GAME_CORE_LIB_VERSION] ," +
                                    "[GAME_LIB_VERSION] ," +
                                    "[MATH_DLL_VERSION] ," +
                                    "[MATH_LIB_VERSION] ," +
                                    "[OS_VERSION] ," +
                                    "[SYSTEM_VERSION] ," +
                                    "[SYSTEM_LIB_A_VERSION] ," +
                                    "[SYSTEM_CORE_LIB_VERSION] ," +
                                    "[InstallDate] ," +
                                    "[RemoveDate] ," +
                                    "[MultiGameEnabled] ," +
                                    "[TransactionPortalIpAddress] ," +
                                    "[TransactionPortalControlPort] ," +
                                    "[ModifiedDate]) " +
                                 "VALUES (" +
                                    "@MachNo ," +
                                    "'339577' ," +
                                    "@Description ," +
                                    "'S' ," +
                                    "'9X1' ," +
                                    "@MachNo," +
                                    "@LocationMachNo ," +
                                    "@IPAddress ," +
                                    "0 ," +
                                    "NULL ," +
                                    "1 ," +
                                    "0 ," +
                                    "0.00 " +
                                    ",0.00 " +
                                    ",NULL ," +
                                    "NULL ," +
                                    "NULL ," +
                                    "@SerialNumber " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 ," +
                                    "' ' " +
                                    ",' ' " +
                                    ",' ' " +
                                    ",' ' " +
                                    ",' ' " +
                                    ",' ' " +
                                    ",' ' " +
                                    ",' ' " +
                                    ",' ' " +
                                    "," +
                                    "'2022-09-10 20:55:08.497' " +
                                    ",NULL " +
                                    ",0 " +
                                    ",NULL " +
                                    ",NULL " +
                                    "," +
                                    "'2022-09-12 14:11:27.083') " +
                              "end " +
                         "end";
            
            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = machNo;
            command.Parameters.Add("@LocationMachNo", System.Data.SqlDbType.VarChar).Value = locMachineNum;
            command.Parameters.Add("@SerialNumber", System.Data.SqlDbType.VarChar).Value = sn;
            command.Parameters.Add("@IPAddress", System.Data.SqlDbType.VarChar).Value = ipAddress;
            command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar).Value = Description;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task DeleteMachine(string machNo)
        {
            var query = "delete from MACH_SETUP where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = machNo;


            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task<List<Bank>> GetAllBanks(bool filterActive = true)
        {
            
            var query = "select * from BANK where IS_ACTIVE = @IsActive";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@IsActive", System.Data.SqlDbType.VarChar).Value = filterActive;

            var reader = await command.ExecuteReaderAsync();

            var banks = new List<Bank>();

            while (reader.Read())
            {
                int bankNum = reader.GetInt32(0);

                banks.Add(new Bank() { BankNumber = bankNum });
            }

            return banks;
        }


        public async Task<Bank> GetBankByBankNumber(int bankNo)
        {

            var query = "select * from BANK where BANK_NO = @BankNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@BankNo", System.Data.SqlDbType.Int).Value = bankNo;

            var reader = await command.ExecuteReaderAsync();

            var bank = new Bank();

            while (reader.Read())
            {
                
                bank.BankNumber = reader.GetInt32(0);
                bank.Description = reader.GetString(1);
                bank.GameTypeCode = reader.GetString(6);
                bank.IsActive = reader.GetBoolean(13);
            }

            return bank;
        }


        public async Task UpdateBankIsActive(int bankNum, bool isActive)
        {

            var query = "update BANK set IS_ACTIVE = @IsActive where BANK_NO = @BankNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@BankNo", System.Data.SqlDbType.Int).Value = bankNum;
            command.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = isActive;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task<List<Game>> GetGamesForBankNumber(int bankNum)
        {

            var query = "select * from GAME_SETUP where GAME_TYPE_CODE = (select GAME_TYPE_CODE from BANK where BANK_NO = @BankNo)";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@BankNo", System.Data.SqlDbType.Int).Value = bankNum;

            var reader = await command.ExecuteReaderAsync();

            var games = new List<Game>();

            while (reader.Read())
            {
                var newGame = new Game();

                newGame.GameCode = reader.GetString(0);
                newGame.GameDescription = reader.GetString(1);
                newGame.TypeId = reader.GetString(2)[0];
                newGame.GameTypeCode = reader.GetString(3);

                games.Add(newGame);
            }

            return games;
        }


        public async Task<List<Game>> GetGamesAssignedToMachine(string machNo)
        {

            var query = "select * from MACH_SETUP_GAMES " +
                            "inner join GAME_Setup gs on " +
                            "MACH_SETUP_GAMES.GAME_CODE = gs.GAME_CODE " +
                        "where MACH_NO = @MachNo and IsEnabled = 1";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.Int).Value = machNo;

            var reader = await command.ExecuteReaderAsync();

            var games = new List<Game>();

            while (reader.Read())
            {
                var newGame = new Game();

                newGame.GameCode = reader.GetString(2);
                newGame.GameDescription = reader.GetString(10);
                newGame.TypeId = reader.GetString(4)[0];
                newGame.GameTypeCode = reader.GetString(12);

                games.Add(newGame);
            }

            return games;
        }


        public async Task UpdateMachineMultiGameEnabled(string MachNo, bool multiGameEnabled)
        {

            var query = "update MACH_SETUP set MultiGameEnabled = @IsEnabled where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = MachNo;
            command.Parameters.Add("@IsEnabled", System.Data.SqlDbType.Bit).Value = multiGameEnabled;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task AddTestDeal()
        {
            string currentDateString = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff");
            DateTime currentDate = DateTime.ParseExact(currentDateString, "yyyy-MM-dd HH-mm-ss.fff", CultureInfo.InvariantCulture);

            var dealStatsQuery = "update DEAL_STATS " +
                                    "set " +
                                    "LAST_PLAY = @LastPlay, " +
                                    "PLAY_COUNT = 1000" +
                                  "where DEAL_NO = @DealNo";

            SqlCommand command = new SqlCommand(dealStatsQuery, DbConnection);
            command.Parameters.Add("@DealNo", System.Data.SqlDbType.Int).Value = TestData.TestDealNumber;
            command.Parameters.Add("@LastPlay", System.Data.SqlDbType.Date).Value = currentDate;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task ResetTestDeal()
        {
            string currentDateString = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff");
            DateTime currentDate = DateTime.ParseExact(currentDateString, "yyyy-MM-dd HH-mm-ss.fff", CultureInfo.InvariantCulture);

            var dealSetupReset = "update DEAL_SETUP " +
                        "SET " +
                        "IS_OPEN = 1" +
                        "WHERE DEAL_NO = @DealNo";

            var dealStatsReset = "update DEAL_STATS " +
                                    "set " +
                                    "LAST_PLAY = @LastPlay, " +
                                    "PLAY_COUNT = 1000 " +
                                  "where DEAL_NO = @DealNo";

            SqlCommand command = new SqlCommand(dealSetupReset, DbConnection);
            command.Parameters.Add("@DealNo", System.Data.SqlDbType.Int).Value = TestData.TestDealNumber;
            command.Parameters.Add("@LastPlay", System.Data.SqlDbType.Date).Value = currentDate;

            var result = await command.ExecuteNonQueryAsync();

            command.CommandText = dealStatsReset;
            result = await command.ExecuteNonQueryAsync();

        }


        public async Task UpdateDealLastPlayMinusDays(int subtractDays,int dealNum)
        {
            
            var query = "update DEAL_STATS " +
                        "set " +
                        "LAST_PLAY = " +
                            "(select DATEADD(day,@DayCount,(select MAX(DTIMESTAMP) from CASINO_TRANS))) " +
                        "where DEAL_NO = @DealNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@DealNo", System.Data.SqlDbType.Int).Value = dealNum;
            command.Parameters.Add("@DayCount", System.Data.SqlDbType.Int).Value = subtractDays;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task UpdateDealPlayCountPercent(double percentComplete, int dealNum)
        {

            var query = "update DSTATS " +
                        "set " +
                        "DSTATS.PLAY_COUNT = ((DS.TABS_PER_ROLL * ds.NUMB_ROLLS) * @PercentComplete) " +
                        "from DEAL_STATS as DSTATS " +
                        "inner join " +
                        "DEAL_SETUP as DS on DSTATS.DEAL_NO = DS.DEAL_NO " +
                        "where DSTATS.DEAL_NO = @DealNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@DealNo", System.Data.SqlDbType.Int).Value = dealNum;
            command.Parameters.Add("@PercentComplete", System.Data.SqlDbType.Decimal).Value = percentComplete;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task UpdateDealPlayCount(int playCount, int dealNum)
        {

            var query = "update DEAL_STATS set PLAY_COUNT = @PlayCount  where DEAL_NO = @DealNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@DealNo", System.Data.SqlDbType.Int).Value = dealNum;
            command.Parameters.Add("@PlayCount", System.Data.SqlDbType.Int).Value = playCount;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task UpdateDealEnabled(int dealNum, bool isEnabled)
        {

            var query = "update DEAL_SETUP set IS_OPEN = @IsOpen where DEAL_NO = @DealNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@DealNo", System.Data.SqlDbType.Int).Value = dealNum;
            command.Parameters.Add("@IsOpen", System.Data.SqlDbType.Bit).Value = isEnabled;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task ExecuteRecommendDealForClosePrecedure()
        {
            SqlCommand cmd = new SqlCommand("dbo.Recommend_Deal_For_Close", DbConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.ExecuteNonQuery();
        }


        public async Task DeleteMachineLastPlay(string machNo)
        {

            var query = "delete from MACH_LAST_PLAY where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = machNo;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task UpdateMachineBalance(string machNo,double balance)
        {

            var query = "update MACH_SETUP set balance = @Balance where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = machNo;
            command.Parameters.Add("@Balance", System.Data.SqlDbType.Decimal).Value = balance;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task UpdateMachineLastPlay(string machNo)
        {

            string currentDateString = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff");
            DateTime currentDate = DateTime.ParseExact(currentDateString, "yyyy-MM-dd HH-mm-ss.fff", CultureInfo.InvariantCulture);

            var query = "BEGIN " +
                            "IF NOT EXISTS " +
                            "(select * from MACH_LAST_PLAY " +
                            "where MACH_NO = @MachNo) " +
                            "BEGIN " +
                                "insert into MACH_LAST_PLAY " +
                                    "([MACH_NO] " +
                                    ",[DEAL_NO] " +
                                    ",[ROLL_NO] " +
                                    ",[TICKET_NO] " +
                                    ",[BARCODE_SCAN] " +
                                    ",[COINS_BET] " +
                                    ",[LINES_BET] " +
                                    ",[ERROR_NO] " +
                                    ",[SEQUENCE_NO] " +
                                    ",[BALANCE] " +
                                    ",[DTIMESTAMP]) " +
                            "values " +
                                    "(@MachNo " +
                                    ",@DealNo " +
                                    ",1 " +
                                    ",260 " +
                                    ",'08CeUXXDD3F1EKQgN88BKMQTK72eXQ5Z' " +
                                    ",2 " +
                                    ",20 " +
                                    ",119 " +
                                    ",7 " +
                                    ",100 " +
                                    ",@TImeStamp) " +
                            "END " +
                         "END";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = machNo;
            command.Parameters.Add("@DealNo", System.Data.SqlDbType.Decimal).Value = TestData.TestDealNumber;
            command.Parameters.Add("@TimeStamp", System.Data.SqlDbType.DateTime).Value = currentDate;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task SetMachineInUse(string machNo)
        {
            await UpdateMachineBalance(machNo, 1.00);
            await UpdateMachineLastPlay(machNo);
        }


        public async Task<int> GetMachineSequenceNumber(string machNo)
        {

            var query = "select SEQUENCE_NO from MACH_LAST_PLAY where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.Int).Value = machNo;

            var reader = await command.ExecuteReaderAsync();

            int sequenceNum = 0;

            while (reader.Read())
            {
                sequenceNum = reader.GetInt32(0);
            }

            return sequenceNum;
        }


        public async Task<string> GetMachineDescription(string machNo)
        {

            var query = "select MODEL_DESC from MACH_SETUP where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.Int).Value = machNo;

            var reader = await command.ExecuteReaderAsync();

            string description = string.Empty;

            while (reader.Read())
            {
                description = reader.GetString(0);
            }

            return description;
        }


        public async Task ResetLocation(string DgeId, int locationId,string LocationName, int RetailerNumber,
            bool isDefault,DateTime AccountDayStart, DateTime AccountDayEnd, string sweepAccount)
        {
            var query = "update CASINO SET " +
                            "CAS_ID = @DGEId, " +
                            "Location_ID = @LocationId, " +
                            "CAS_NAME = @LocationName, " +
                            "RETAILER_NUMBER = @RetailerNum, " +
                            "SETASDEFAULT = @IsDefault, " +
                            "FROM_TIME = @StartTime, " +
                            "TO_TIME = @EndTime, " +
                            "SWEEP_ACCT = @SweepAccount " +
                         "where " +
                         "LOCATION_ID = @LocationId";

            var setDefaultQuery = "update CASINO set SETASDEFAULT = case when LOCATION_ID = @LocationId then 1 else 0 end";

            var deleteTestLocationQuery = "delete from CASINO where Location_ID = @TestLocationId";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@DGEId", System.Data.SqlDbType.VarChar).Value = DgeId;
            command.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = locationId;
            command.Parameters.Add("@LocationName", System.Data.SqlDbType.VarChar).Value = LocationName;
            command.Parameters.Add("@RetailerNum", System.Data.SqlDbType.Int).Value = RetailerNumber;
            command.Parameters.Add("@IsDefault", System.Data.SqlDbType.Bit).Value = isDefault;
            command.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime).Value = AccountDayStart;
            command.Parameters.Add("@EndTime", System.Data.SqlDbType.DateTime).Value = AccountDayEnd;
            command.Parameters.Add("@SweepAccount", System.Data.SqlDbType.VarChar).Value = sweepAccount;
            command.Parameters.Add("@TestLocationId", System.Data.SqlDbType.Int).Value = TestData.TestLocationId;

            var reader = await command.ExecuteNonQueryAsync();

            command.CommandText = setDefaultQuery;
            reader = await command.ExecuteNonQueryAsync();

            command.CommandText = deleteTestLocationQuery;
            reader = await command.ExecuteNonQueryAsync();
        }


        public async Task<Location> GetLocation(int locId)
        {

            var query = "select " +
                            "CAS_ID, " +
                            "Location_ID, " +
                            "CAS_NAME, " +
                            "RETAILER_NUMBER, " +
                            "SETASDEFAULT, " +
                            "FROM_TIME, " +
                            "TO_TIME," +
                            "SWEEP_ACCT " +
                         "from CASINO " +
                         "where " +
                         "LOCATION_ID = @LocationId";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = locId;

            var reader = await command.ExecuteReaderAsync();

            Location location = new Location();

            while (reader.Read())
            {
                location.DgeId = reader.GetString(0);
                location.LocationId = reader.GetInt32(1);
                location.LocationName = reader.GetString(2);
                location.RetailerNumber = int.Parse(reader.GetString(3));
                location.IsDefault = reader.GetBoolean(4);
                location.AccountDayStart = reader.GetDateTime(5);
                location.AccountDayEnd = reader.GetDateTime(6);
                location.SweepAmount = reader.GetString(7);
            }

            return location;
        }


        public async Task<PromoEntrySchedule> GetPromoById(int promoId)
        {

            var query = "select * from PROMO_SCHEDULE where PromoScheduleId = @PromoId";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@PromoId", System.Data.SqlDbType.Int).Value = promoId;

            var reader = await command.ExecuteReaderAsync();

            PromoEntrySchedule promo = new PromoEntrySchedule();

            while (reader.Read())
            {
                promo.Id = reader.GetInt32(0);
                promo.Description = reader.GetString(1);
                promo.StartTime = reader.GetDateTime(2);
                promo.EndTime = reader.GetDateTime(3);
                promo.Started = reader.GetBoolean(4);
                promo.Ended = reader.GetBoolean(5);
                promo.PromoTicketCount = reader.GetInt32(6);
                promo.PromoFactorCount = reader.GetInt32(7);
            }

            return promo;
        }


        public async Task<PromoEntrySchedule> ResetTestPromo()
        {
            var startHour = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00.000");
            var startDate = DateTime.ParseExact(startHour, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            var oneHourLater = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:00:00.000");
            var endDate = DateTime.ParseExact(oneHourLater, "yyyy-MM-dd HH:00:00.000", CultureInfo.InvariantCulture);

            var query = "update PROMO_SCHEDULE " +
                        "set " +
                        "Comments = 'Automation Test Promo', " +
                        "PromoStart = @StartTime, " +
                        "PromoEnd = @EndTime, " +
                        "PromoStarted = 0, " +
                        "PromoEnded = 0, " +
                        "TOTAL_PROMO_AMOUNT_TICKETS = 0, " +
                        "TOTAL_PROMO_FACTOR_TICKETS = 0 " +
                        "where PromoScheduleID = @PromoId";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@PromoId", System.Data.SqlDbType.Int).Value = TestData.TestPromoEntryScheduleId;
            command.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime).Value = startDate;
            command.Parameters.Add("@EndTime", System.Data.SqlDbType.DateTime).Value = endDate;

            var reader = await command.ExecuteReaderAsync();

            PromoEntrySchedule promo = new PromoEntrySchedule();

            while (reader.Read())
            {
                promo.Id = reader.GetInt32(0);
                promo.Description = reader.GetString(1);
                promo.StartTime = reader.GetDateTime(2);
                promo.EndTime = reader.GetDateTime(3);
                promo.Started = reader.GetBoolean(4);
                promo.Ended = reader.GetBoolean(5);
                promo.PromoTicketCount = reader.GetInt32(6);
                promo.PromoFactorCount = reader.GetInt32(7);
            }

            return promo;
        }



        public async Task<PromoEntrySchedule> UpdatePromoScheduleDates(DateTime startDate, DateTime endDate, int promoId)
        {
           

            var query = "update PROMO_SCHEDULE " +
                        "set " +
                        "PromoStart = @StartTime, " +
                        "PromoEnd = @EndTime " +
                        "where PromoScheduleID = @PromoId";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@PromoId", System.Data.SqlDbType.Int).Value = promoId;
            command.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime).Value = startDate;
            command.Parameters.Add("@EndTime", System.Data.SqlDbType.DateTime).Value = endDate;

            var reader = await command.ExecuteReaderAsync();

            PromoEntrySchedule promo = new PromoEntrySchedule();

            while (reader.Read())
            {
                promo.Id = reader.GetInt32(0);
                promo.Description = reader.GetString(1);
                promo.StartTime = reader.GetDateTime(2);
                promo.EndTime = reader.GetDateTime(3);
                promo.Started = reader.GetBoolean(4);
                promo.Ended = reader.GetBoolean(5);
                promo.PromoTicketCount = reader.GetInt32(6);
                promo.PromoFactorCount = reader.GetInt32(7);
            }

            return promo;
        }


        public async Task UpdatePromo(int promoId, DateTime startDate, 
            DateTime endDate, bool hasStarted, bool hasEnded)
        {
            

            var query = "update PROMO_SCHEDULE " +
                        "set " +
                       
                        "PromoStart = @StartTime, " +
                        "PromoEnd = @EndTime, " +
                        "PromoStarted = @IsStarted, " +
                        "PromoEnded = @IsEnded " +
                        "where PromoScheduleID = @PromoId";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@PromoId", System.Data.SqlDbType.Int).Value = promoId;
            command.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime).Value = startDate;
            command.Parameters.Add("@EndTime", System.Data.SqlDbType.DateTime).Value = endDate;
            command.Parameters.Add("@IsStarted", System.Data.SqlDbType.Bit).Value = hasStarted;
            command.Parameters.Add("@IsEnded", System.Data.SqlDbType.Bit).Value = hasEnded;

            var reader = await command.ExecuteNonQueryAsync();
        }


        public async Task DeletePromo(string description)
        {


            var query = "delete from PROMO_SCHEDULE where Comments = @Description";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar).Value = description;

            var reader = await command.ExecuteNonQueryAsync();
        }


        /* query to set play count > 98%
         * update DSTATS set DSTATS.PLAY_COUNT = ((DS.TABS_PER_ROLL * ds.NUMB_ROLLS) * 0.99) from DEAL_STATS as DSTATS inner join DEAL_SETUP as DS on DSTATS.DEAL_NO = DS.DEAL_NO where DSTATS.DEAL_NO = 1000
         */
    }
}
