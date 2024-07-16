using Dapper;
using Newtonsoft.Json.Linq;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.ApplicationInsights;

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
        public List<T> ExecuteStoredProcedure<T>(string storedProcedureName, Func<SqlDataReader, T> mapFunction, DynamicParameters parameters = null)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters.ParameterNames.Select(name => new SqlParameter(name, parameters.Get<object>(name))).ToArray());

                        // Log parameters to Application Insights
                        LogParametersToApplicationInsights(storedProcedureName, parameters);
                    }
                    
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<T> result = new List<T>();

                        while (reader.Read())
                        {
                            T item = mapFunction(reader);
                            result.Add(item);
                        }
                        return result;
                    }
                }
            }
        }

        public void LogParametersToApplicationInsights(string storedProcedureName, DynamicParameters parameters)
        {
            
            TelemetryClient telemetryClient = new ();

            telemetryClient.TrackTrace($"Executing stored procedure: {storedProcedureName}");

           
            foreach (var paramName in parameters.ParameterNames)
            {
                var paramValue = parameters.Get<object>(paramName);
                telemetryClient.TrackTrace($"Parameter: {paramName} Value: {paramValue}");
                Console.WriteLine( $"ParameterName:{paramName} Value:{paramValue}");
            }
        }

    }

}