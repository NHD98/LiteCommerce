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
    public class CustomerDAL : ICustomerDAL
    {
        private string connectionString;

        public CustomerDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Customer data)
        {
            int customerInsert = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Customers
                                          (
                                              CustomerID,
	                                          CompanyName,
	                                          ContactName,
	                                          ContactTitle,
	                                          Address,
	                                          City,
	                                          Country,
	                                          Phone,
	                                          Fax
                                          )
                                          VALUES
                                          (
                                              @CustomerID,
	                                          @CompanyName,
	                                          @ContactName,
	                                          @ContactTitle,
	                                          @Address,
	                                          @City,
	                                          @Country,
	                                          @Phone,
	                                          @Fax
                                          );";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
                cmd.Parameters.AddWithValue("@ContactTitle", data.ContactTitle);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                cmd.Parameters.AddWithValue("@Fax", data.Fax);

                customerInsert = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return customerInsert;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue, string country)
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
                cmd.CommandText = "select COUNT(*) from Customers where (@searchValue = N'' or ContactName like @searchValue) and (@country = N'' or Country = @country)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@country", country);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerIDs"></param>
        /// <returns></returns>
        public int Delete(string[] CustomerIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Customers
                                    WHERE(CustomerID = @CustomerID) AND CustomerID NOT IN (
                                        SELECT DISTINCT CustomerID FROM Orders
                                    )";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@CustomerID", SqlDbType.NChar);
                foreach (string customerID in CustomerIDs)
                {
                    cmd.Parameters["@CustomerID"].Value = customerID;
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
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public Customer Get(string CustomerID)
        {
            Customer data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Customers WHERE CustomerID = @customerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@customerID", CustomerID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Customer()
                        {
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            ContactName = Convert.ToString(dbReader["ContactName"]),
                            ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Phone = Convert.ToString(dbReader["Phone"]),
                            Fax = Convert.ToString(dbReader["Fax"])
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
        public List<Customer> List(int page, int pageSize, string searchValue, string country)
        {

            List<Customer> data = new List<Customer>();
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
	                                    select ROW_NUMBER() over(order by ContactName) as RowNumber, Customers.*
	                                    from Customers
	                                    where (@searchValue = N'' or ContactName like @searchValue)
                                            and (@country = N'' or Country = @country)
                                    ) as t
                                    where t.RowNumber between @pageSize * (@page -  1) + 1 and @page * @pageSize";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@country", country);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Customer()
                        {
                            CustomerID = dbReader["CustomerID"].ToString(),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            ContactName = Convert.ToString(dbReader["ContactName"]),
                            ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Phone = Convert.ToString(dbReader["Phone"]),
                            Fax = Convert.ToString(dbReader["Fax"])
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
        public bool Update(Customer data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Customers
                                           SET CompanyName = @CompanyName 
                                              ,ContactName = @ContactName
                                              ,ContactTitle = @ContactTitle
                                              ,Address = @Address
                                              ,City = @City
                                              ,Country = @Country
                                              ,Phone = @Phone
                                              ,Fax = @Fax
                                          WHERE CustomerID = @CustomerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
                cmd.Parameters.AddWithValue("@ContactTitle", data.ContactTitle);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                cmd.Parameters.AddWithValue("@Fax", data.Fax);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
