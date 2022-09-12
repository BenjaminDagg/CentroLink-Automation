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

            var query = "update MACH_SETUP " +
                "set REMOVED_FLAG = 0, " +
                "ACTIVE_FLAG = 1, CASINO_MACH_NO = @LocationMachineNumber " +
                "where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = TestData.DefaultMachineNumber;
            command.Parameters.Add("@LocationMachineNumber", System.Data.SqlDbType.VarChar).Value = TestData.DefaultLocationMachineNumber;

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
    }
}
