using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class AttributeDAL : IAttributeDAL
    {
        private string connectionString;
        public AttributeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<DomainModels.Attribute> List()
        {
            List<DomainModels.Attribute> list = new List<DomainModels.Attribute>();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Attributes";
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                using (SqlDataReader dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        list.Add(new DomainModels.Attribute()
                        {
                            AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                            AttributeName = Convert.ToString(dbReader["AttributeName"])
                        });
                    }
                }
                connection.Close();
            }

            return list;
        }
    }
}
