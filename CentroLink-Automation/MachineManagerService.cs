using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CentroLink_Automation
{
    public class MachineManagerService
    {
        public Machine Machine {get;set;}
        private SqlConnection DbConnection;
        
        public static async Task<MachineManagerService> MachineManagerServiceAsync(int machNo, SqlConnection conn)
        {
            var mach = await GetMachineData(machNo,conn);
            return new MachineManagerService(mach,conn); 
        }


        private MachineManagerService(Machine mach, SqlConnection conn)
        {
            this.Machine = mach;
            this.DbConnection = conn;
        }


        private static async Task<Machine> GetMachineData(int machNo, SqlConnection conn)
        {
            var query = "select " +
                        "MS.MACH_NO, MS.LOCATION_ID, MS.MODEL_DESC, MS.ACTIVE_FLAG, " +
                        "MS.IP_ADDRESS, MS.REMOVED_FLAG ,MS.MACH_SERIAL_NO,MS.BALANCE,MS.PROMO_BALANCE, " +
                        "MLP.SEQUENCE_NO " +
                        "from MACH_SETUP as MS " +
                        "inner join MACH_LAST_PLAY MLP " +
                        "on MS.MACH_NO = MLP.MACH_NO " +
                        "where MS.MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.Int).Value = machNo;

            var reader = await command.ExecuteReaderAsync();

            Machine mach = new Machine();

            while (reader.Read())
            {
                mach.MachineNumber = reader.GetString(0);
                mach.LocationMachineNumber = reader.GetInt32(1).ToString();
                mach.Description = reader.GetString(2);
                mach.Status = reader.GetByte(3) == 1 ? true : false;
                mach.IPAddress = reader.GetString(4);
                mach.Removed = reader.GetBoolean(5);
                mach.SerialNumber = reader.GetString(6);
                mach.Balance = (double)reader.GetDecimal(7);
                mach.PromoBalance = (double)(reader.GetDecimal(8));
                mach.SequenceNumber = (reader.GetInt32(9)) + 1;
            }

            return mach;
        }


        public void ResetMachine()
        {
            var query = "update MACH_LAST_PLAY set SEQUENCE_NO = @SeqNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@SeqNo", System.Data.SqlDbType.Int).Value = Machine.SequenceNumber;

            var reader = command.ExecuteReader();
        }


        public async Task<Machine> RetrieveMachine()
        {
            var query = "select " +
                        "MS.MACH_NO, MS.LOCATION_ID, MS.MODEL_DESC, MS.ACTIVE_FLAG, " +
                        "MS.IP_ADDRESS, MS.REMOVED_FLAG ,MS.MACH_SERIAL_NO,MS.BALANCE,MS.PROMO_BALANCE, " +
                        "MLP.SEQUENCE_NO " +
                        "from MACH_SETUP as MS " +
                        "inner join MACH_LAST_PLAY MLP " +
                        "on MS.MACH_NO = MLP.MACH_NO " +
                        "where MS.MACH_NO = @MachNo";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@MachNo", System.Data.SqlDbType.Int).Value = Machine.MachineNumber;

            var reader = await command.ExecuteReaderAsync();

            Machine mach = new Machine();

            while (reader.Read())
            {
                mach.MachineNumber = reader.GetString(0);
                mach.LocationMachineNumber = reader.GetInt32(1).ToString();
                mach.Description = reader.GetString(2);
                mach.Status = reader.GetByte(3) == 1 ? true : false;
                mach.IPAddress = reader.GetString(4);
                mach.Removed = reader.GetBoolean(5);
                mach.SerialNumber = reader.GetString(6);
                mach.Balance = (double)reader.GetDecimal(7);
                mach.PromoBalance = (double)(reader.GetDecimal(8));
                mach.SequenceNumber = (reader.GetInt32(9)) + 1;
            }

            this.Machine = mach;

            return mach;
        }
    }
}
