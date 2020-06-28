using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class EmployeeUserAccountDAL : IUserAccountDAL
    {
        private string connectionString;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public EmployeeUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// xac thuc 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserAccount Authorize(string userName, string password)
        {
            UserAccount user = null;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT Title, FirstName, LastName, PhotoPath, EmployeeID, Roles FROM Employees WHERE Email = @Email and Password = @Password";
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Email", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                using (SqlDataReader dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        user = new UserAccount()
                        {
                            Title = Convert.ToString(dbReader["Title"]),
                            FullName = Convert.ToString(dbReader["FirstName"]) + " " + Convert.ToString(dbReader["LastName"]),
                            Photo = Convert.ToString(dbReader["PhotoPath"]),
                            UserID = Convert.ToString(dbReader["EmployeeID"]),
                            Roles = Convert.ToString(dbReader["Roles"])
                        };
                    }
                }
                connection.Close();
            }
            return user;
        }
    }
}
