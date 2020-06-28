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
    public class CountryDAL : ICountryDAL
    {
        private string connectionString;
        /// <summary>
        /// khoi tao
        /// </summary>
        /// <param name="connectionString"></param>
        public CountryDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// danh sach quoc gia
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public List<Country> List(int page, int pageSize, string searchValue)
        {
            List<Country> list = new List<Country>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from (
	                                    select ROW_NUMBER() over(order by CountryName) as RowNumber, Countries.*
	                                    from Countries
	                                    where (@searchValue = N'') or (CountryName like @searchValue)
                                    ) as t
                                    where (@pageSize <= 0) or
                                        (t.RowNumber between @pageSize * (@page -  1) + 1 and @page * @pageSize)";
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@page", page);
                using (SqlDataReader dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        list.Add(new Country()
                        {
                            CountryName = Convert.ToString(dbReader["CountryName"]),
                            CountryID = Convert.ToString(dbReader["CountryID"])
                        });
                    }
                }
                connection.Close();
            }

            return list;
        }
        /// <summary>
        /// dem quoc gia
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
                cmd.CommandText = "select COUNT(*) from Countries where @searchValue = N'' or CountryName like @searchValue";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return count;
        }
        /// <summary>
        /// lay chi tiet quoc gia
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        public Country Get(string countryID)
        {
            Country data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Countries WHERE CountryID = @countryID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@countryID", countryID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Country()
                        {
                            CountryID = Convert.ToString(dbReader["CountryID"]),
                            CountryName = Convert.ToString(dbReader["CountryName"])
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// them quoc gia
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Add(Country data)
        {
            int rowEffect = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Countries
                                          (
	                                          CountryID,
                                              CountryName
                                          )
                                          VALUES
                                          (
	                                          @CountryID,
	                                          @CountryName
                                          )";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CountryID", data.CountryID);
                cmd.Parameters.AddWithValue("@CountryName", data.CountryName);

                rowEffect = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowEffect > 0;
        }
        /// <summary>
        /// cap nhat quoc gia
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Country data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Countries
                                           SET CountryName = @CountryName
                                          WHERE CountryID = @CountryID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CountryID", data.CountryID);
                cmd.Parameters.AddWithValue("@CountryName", data.CountryName);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
        /// <summary>
        /// xoa quoc gia
        /// </summary>
        /// <param name="countryIDs"></param>
        /// <returns></returns>
        public int Delete(string[] countryIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Countries
                                            WHERE(CountryID = @CountryID)
                                              AND(CountryName NOT IN(SELECT Country FROM Customers))
                                                AND(CountryName NOT IN(SELECT ShipCountry FROM Orders))
                                                AND(CountryName NOT IN(SELECT Country FROM Employees))
                                                AND(CountryName NOT IN(SELECT Country FROM Suppliers))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@CountryID", SqlDbType.NVarChar);
                foreach (string countryID in countryIDs)
                {
                    cmd.Parameters["@CountryID"].Value = countryID;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }
    }
}
