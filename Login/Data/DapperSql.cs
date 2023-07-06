using Dapper;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;

namespace Login.Data
{
    public class DapperSql
    {
        private SqlConnection _connection;
        private void Init()
        {
            _connection = new SqlConnection(GetConnectionString());
        }



        private string GetConnectionString()
        {
            var targetPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory()));
            string appSettingsPath = Path.Combine(targetPath, "appsettings.json");
            string json = File.ReadAllText(appSettingsPath);
            var jObject = JObject.Parse(json);
            string connectionString = jObject.GetValue("ConnectionStrings")["DefaultConnection"].ToString();
            return connectionString;
        }



        public T LoadSPDataSingleWithParam<T>(string sql, DynamicParameters param)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                var result = cnn.Query<T>(sql, param, commandType: CommandType.StoredProcedure).SingleOrDefault();
                return result;
            }
        }
        public List<T> LoadSPDataListWithParam<T>(string sql, DynamicParameters param)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                var result = cnn.Query<T>(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }
        public T LoadSQLDataSingle<T>(string sql)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql).SingleOrDefault();
            }
        }
        public List<T> LoadSQLDataList<T>(string sql)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                var result = cnn.Query<T>(sql).ToList();
                return result;
            }
        }
        public List<T> LoadSQLDataListWithParam<T>(string sql, DynamicParameters p)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql, p).ToList();
            }
        }
        public T LoadSQLDataSingleWithParam<T>(string sql, DynamicParameters p)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql, p).SingleOrDefault();
            }
        }
        public DataTable ExecuteDataTable(string sql)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public int RunSQLWithParam(string sql, DynamicParameters p)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql, p);
            }
        }
    }

}
