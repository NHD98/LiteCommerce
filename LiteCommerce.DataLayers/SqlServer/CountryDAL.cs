using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class CountryDAL : ICountryDAL
    {
        private string connectionString;
        public CountryDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Country> List()
        {
            List<Country> list = new List<Country>();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Countries";
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                using (SqlDataReader dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        list.Add(new Country()
                        {
                            CountryID = Convert.ToString(dbReader["CountryID"]),
                            CountryName = Convert.ToString(dbReader["CountryName"])
                        });
                    }
                }
                connection.Close();
            }

            return list;
        }
    }
}
