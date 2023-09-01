using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TastyTreats.Types;

namespace DAL
{
    public class DataAccess
    {
        #region Fields

        private readonly string connectionString;

        #endregion

        #region Constructors

        public DataAccess()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            connectionString = builder.Build().GetConnectionString("TastyTreatsConnStr");
        }

        #endregion

        #region Public Methods

        public DataTable Execute(string cmdText, List<ParmStruct>? parms = null, CommandType cmdType = CommandType.StoredProcedure)
        {
            DataTable dt = new();
            SqlCommand cmd = CreateCommand(cmdText, cmdType, parms);

            using (cmd.Connection)
            {
                SqlDataAdapter da = new(cmd);

                da.Fill(dt);
            }

            return dt;
        }

        public async Task<DataTable> ExecuteAsync(string cmdText, List<ParmStruct>? parms = null, CommandType cmdType = CommandType.StoredProcedure)
        {
            DataTable dt = new();
            SqlCommand cmd = CreateCommand(cmdText, cmdType, parms);

            using (cmd.Connection)
            {
                SqlDataAdapter da = new(cmd);

                await Task.Run(() => da.Fill(dt));
            }

            return dt;
        }

        public object ExecuteScalar(string cmdText, List<ParmStruct>? parms = null, CommandType cmdType = CommandType.StoredProcedure)
        {
            object retVal;
            SqlCommand cmd = CreateCommand(cmdText, cmdType, parms);

            using (cmd.Connection)
            {
                cmd.Connection.Open();
                retVal = cmd.ExecuteScalar();
            }

            return retVal;
        }

        public async Task<object?> ExecuteScalarAsync(string cmdText, List<ParmStruct>? parms = null, CommandType cmdType = CommandType.StoredProcedure)
        {
            object? retVal;
            SqlCommand cmd = CreateCommand(cmdText, cmdType, parms);

            using (cmd.Connection)
            {
                cmd.Connection.Open();
                retVal = await cmd.ExecuteScalarAsync();
            }

            return retVal;
        }

        public int ExecuteNonQuery(string cmdText, List<ParmStruct>? parms = null, CommandType cmdType = CommandType.StoredProcedure)
        {
            int rowsAffected = 0;
            SqlCommand cmd = CreateCommand(cmdText, cmdType, parms);

            using (cmd.Connection)
            {
                cmd.Connection.Open();
                rowsAffected = cmd.ExecuteNonQuery();

                UnloadParms(parms, cmd);
            }

            return rowsAffected;
        }

        public async Task<int> ExecuteNonQueryAsync(string cmdText, List<ParmStruct>? parms = null, CommandType cmdType = CommandType.StoredProcedure)
        {
            int rowsAffected = 0;
            SqlCommand cmd = CreateCommand(cmdText, cmdType, parms);

            using (cmd.Connection)
            {
                await cmd.Connection.OpenAsync();
                rowsAffected = await cmd.ExecuteNonQueryAsync();

                UnloadParms(parms, cmd);
            }

            return rowsAffected;
        }

        #endregion

        #region Private Methods
        private SqlCommand CreateCommand(string cmdText, CommandType cmdType, List<ParmStruct>? parms)
        {
            SqlConnection conn = new(connectionString);

            SqlCommand cmd = new(SQLCleaner(cmdText), conn)
            {
                CommandType = cmdType
            };

            if (parms != null)
            {
                foreach (ParmStruct p in parms)
                {
                    cmd.Parameters.Add(p.Name, p.DataType, p.Size).Value = p.Value;
                    cmd.Parameters[p.Name].Direction = p.Direction;
                }
            }
            return cmd;
        }

        private void UnloadParms(List<ParmStruct> parms, SqlCommand cmd)
        {
            if (parms != null)
            {
                for (int i = 0; i <= parms.Count - 1; i++)
                {
                    ParmStruct p = parms[i];
                    p.Value = cmd.Parameters[i].Value;
                    parms[i] = p;
                }
            }
        }

        private string SQLCleaner(string sql)
        {
            while (sql.Contains("  "))
                sql = sql.Replace("  ", " ");

            return sql.Replace(Environment.NewLine, "");
        }

        #endregion
    }
}
