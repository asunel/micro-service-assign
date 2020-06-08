using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DBTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values/1
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var userDataTable = GetUserById(id);
            var row = userDataTable.Rows[0];

            var user = new Dictionary<string, object>();
            foreach (DataColumn col in userDataTable.Columns)
            {
                user.Add(col.ColumnName.ToLower(), row[col]);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(user);
        }


        public DataTable GetUserById(int id)
        {
            string str = "select Name, Age, Email from [User] where Id=@Id";

            DataTable dt = new DataTable();
            try
            {
                SqlDataReader reader;

                string mssqlDbIp = Environment.GetEnvironmentVariable("MSSQL_DB_IP"); // create a config key MSSQL_DB_IP in GKE with value = External Endpoint IP of MSSQL service
                string userDb = Environment.GetEnvironmentVariable("USER_DB"); // create a config key MSSQL_DB_IP in GKE with value = External Endpoint IP of MSSQL service
                string dbUserName = Environment.GetEnvironmentVariable("DB_USER_NAME"); // create a config key MSSQL_DB_IP in GKE with value = External Endpoint IP of MSSQL service

                SqlConnectionStringBuilder sConnB = new SqlConnectionStringBuilder
                {
                    DataSource = mssqlDbIp, //"35.223.239.227",
                    InitialCatalog = userDb, // "UserDB",
                    UserID = dbUserName, //"sa",
                    Password = "Passw0rd1"
                };

                //var connectionString = "Data Source=.;Initial Catalog=UserDB ;Integrated Security=True;";
                var connectionString = sConnB.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        reader = cmd.ExecuteReader();
                        dt.Load(reader);

                        reader.Close();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return dt;
        }
    }
}
