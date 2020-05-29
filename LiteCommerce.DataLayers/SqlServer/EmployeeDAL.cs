using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;
using System.Data;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private string connectionString;

        public EmployeeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Employee data)
        {
            int EmployeeId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Employees
                                          (
	                                         LastName, FirstName, Title, BirthDate, HireDate, Email, Address, City, Country, HomePhone, Notes, PhotoPath, Password
                                          )
                                          VALUES
                                          (
	                                          @LastName, @FirstName, @Title, @BirthDate, @HireDate, @Email, @Address, @City, @Country, @HomePhone, @Notes, @PhotoPath, @Password
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@HireDate", data.HireDate);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Password", data.LastName);

                EmployeeId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return EmployeeId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select COUNT(*) from Employees where @searchValue = N'' or (FirstName like @searchValue) or (LastName like @searchValue)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmployeeIDs"></param>
        /// <returns></returns>
        public int Delete(int[] EmployeeIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Employees
                                            WHERE(EmployeeID = @EmployeeID)
                                              AND(EmployeeID NOT IN(SELECT EmployeeID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
                foreach (int employeeID in EmployeeIDs)
                {
                    cmd.Parameters["@EmployeeID"].Value = employeeID;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public Employee Get(int EmployeeID)
        {
            Employee data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Employees WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            Password = Convert.ToString(dbReader["Password"])
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public List<Employee> List(int page, int pageSize, string searchValue)
        {

            List<Employee> data = new List<Employee>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Tạo lệnh thực thi truy vấn dữ liệu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from (
	                                    select ROW_NUMBER() over(order by FirstName) as RowNumber, Employees.*
	                                    from Employees
	                                    where (@searchValue = N'') or (FirstName like @searchValue) or (LastName like @searchValue)
                                    ) as t
                                    where t.RowNumber between @pageSize * (@page -  1) + 1 and @page * @pageSize";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            Password = Convert.ToString(dbReader["Password"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            Title = Convert.ToString(dbReader["Title"])
                        });
                    }
                }

                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Employee data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                           SET LastName = @LastName, FirstName = @FirstName, Title = @Title, BirthDate = @BirthDate,
                                                HireDate = @HireDate, Email = @Email, Address = @Address, City = @City, Country = @Country,
                                                HomePhone = @HomePhone, Notes = @Notes, PhotoPath = @PhotoPath, Password = @Password
                                          WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                string birthDate = data.BirthDate.ToString("yyyy-MM-dd");
                string hireDate = data.HireDate.ToString("yyyy-MM-dd");

                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@BirthDate", birthDate);
                cmd.Parameters.AddWithValue("@HireDate", hireDate);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Password", data.Password);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
