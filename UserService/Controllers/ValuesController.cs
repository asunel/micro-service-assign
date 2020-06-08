using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = GetUserById(id);
            return user;
        }

        public User GetUserById(int id)
        {
            string str = "select Name, Age, Email from [User] where Id=@Id";

            DataTable userDataTable = new DataTable();
            try
            {
                SqlDataReader reader;

                string mssqlDbIp = Environment.GetEnvironmentVariable("MSSQL_DB_IP"); // create a config key MSSQL_DB_IP in GKE with value = External Endpoint IP of MSSQL service
                string userDb = Environment.GetEnvironmentVariable("USER_DB");
                string dbUserName = Environment.GetEnvironmentVariable("DB_USER_NAME");
                string dbUserPassword = Environment.GetEnvironmentVariable("PASSWORD");

                //mssqlDbIp = "34.67.200.82";
                //userDb = "UserDB";
                //dbUserName = "sa";
                //dbUserPassword = "UGFzc3cwcmQx";

                //dbUserPassword = "UGFzc3cwcmQx";
                byte[] data = Convert.FromBase64String(dbUserPassword);
                dbUserPassword = Encoding.UTF8.GetString(data);

                SqlConnectionStringBuilder sConnB = new SqlConnectionStringBuilder
                {
                    DataSource = mssqlDbIp,
                    InitialCatalog = userDb,
                    UserID = dbUserName,
                    Password = dbUserPassword
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
                        userDataTable.Load(reader);

                        reader.Close();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }

            User user = null;
            if (userDataTable.Rows != null && userDataTable.Rows.Count > 0)
            {
                user = new User();
                var row = userDataTable.Rows[0];

                if (row != null)
                {
                    if (row["Name"] != null)
                    {
                        user.Name = row["Name"].ToString();
                    }

                    if (row["Age"] != null)
                    {
                        user.Age = Int32.Parse(row["Age"].ToString());
                    }

                    if (row["Email"] != null)
                    {
                        user.Email = row["Email"].ToString();
                    }
                }
            }

            return user;
        }
    }
}
