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
                "ACTIVE_FLAG = 1" +
                "where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = TestData.MachineNumber;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task UpdateMachineRemovedFlag(string machNo, bool isRemoved)
        {

            var query = "update MACH_SETUP set REMOVED_FLAG = @IsActive where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = TestData.MachineNumber;
            command.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = isRemoved;

            var result = await command.ExecuteNonQueryAsync();
        }


        public async Task UpdateMachineActivedFlag(string machNo, bool isActive)
        {

            var query = "update MACH_SETUP set ACTIVE_FLAG = @IsActive where MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.VarChar).Value = TestData.MachineNumber;
            command.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = isActive;

            var result = await command.ExecuteNonQueryAsync();
        }
    }
}
