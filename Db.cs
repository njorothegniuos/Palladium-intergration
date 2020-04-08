using Dapper;
using Palladium.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Palladium
{
    public class Db
    {
        private string connString;
        public Db(string dbConnString)
        {
            this.connString = dbConnString;
        }

        public GenericModel GetNextReceiptNo()
        {
            try
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string sql = "sp_GetNextReceiptNo";

                    return conn.Query<GenericModel>(sql, null, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Util.LogError("GetSettings", ex);
            }

            return null;
        }

    }
}
