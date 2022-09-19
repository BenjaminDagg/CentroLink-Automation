using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CentroLink_Automation
{
    public class GameManagerService
    {
        public Game Game { get; set; }
        private SqlConnection DbConnection;

        public static async Task<GameManagerService> BuildGameManagerServiceAsync(string gtc, SqlConnection conn)
        {
            var game = await GetGameData(gtc,conn);
            return new GameManagerService(game,conn);
        }


        private GameManagerService(Game game,SqlConnection conn)
        {
            this.Game = game;
            this.DbConnection = conn;
        }


        public static async Task<Game> GetGameData(string gtc, SqlConnection conn)
        {
            var query = "select GS.GAME_CODE, GS.GAME_DESC, GS.GAME_TYPE_CODE, " +
                        "GT.BARCODE_TYPE_ID " +
                        "from GAME_SETUP GS " +
                        "inner join GAME_TYPE GT " +
                        "on " +
                        "GT.GAME_TYPE_CODE = GS.GAME_TYPE_CODE " +
                        "where GS.GAME_CODE = @GTC";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.Add("@GTC", System.Data.SqlDbType.VarChar).Value = gtc;

            var reader = await command.ExecuteReaderAsync();

            Game game = new Game();

            while (reader.Read())
            {
               game.GameCode = reader.GetString(0);
               game.GameDescription = reader.GetString(1);
               game.GameTypeCode = reader.GetString(2);
               game.BarcodeTypeId = reader.GetInt16(3);
            }

            return game;
        }


        public int GetPayTierLevel(int coinsWon)
        {
            var query = "select top 1 PS.TIER_LEVEL " +
                            "from PAYSCALE_TIER PS " +
                                "inner join PAYSCALE P " +
                                "on P.PAYSCALE_NAME = PS.PAYSCALE_NAME " +
                                "inner join GAME_SETUP GS " +
                                "on GS.GAME_TYPE_CODE = P.GAME_TYPE_CODE " +
                            "where " +
                            "GS.GAME_CODE = @GTC and  PS.COINS_WON = @CoinsWon order by PS.TIER_LEVEL desc";

            SqlCommand command = new SqlCommand(query, DbConnection);
            command.Parameters.Add("@GTC", System.Data.SqlDbType.VarChar).Value = Game.GameCode;
            command.Parameters.Add("@CoinsWon", System.Data.SqlDbType.Int).Value = coinsWon;

            var reader = command.ExecuteReader();

            int tier = 0;

            while (reader.Read())
            {
                tier = reader.GetInt16(0);
            }

            return tier;
        }
    }
}
