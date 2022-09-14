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
                            "GAME_CODE = @GameCode " +
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
    }
}
