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
    public class ProductAttributeDAL : IProductAttributeDAL
    {
        private string connectionString;
        public ProductAttributeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(List<ProductAttribute> attribute)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO ProductAttributes (
                                    AttributeID, ProductID, AttributeValues, DisplayOrder)
                                    VALUES (@AttributeID, @ProductID, @AttributeValues, @DisplayOrder)
                                    ";
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add("@AttributeID", SqlDbType.BigInt);
                cmd.Parameters.Add("@ProductID", SqlDbType.Int);
                cmd.Parameters.Add("@AttributeValues", SqlDbType.NVarChar);
                cmd.Parameters.Add("@DisplayOrder", SqlDbType.Int);
                foreach (ProductAttribute productAttribute in attribute)
                {
                    cmd.Parameters["@AttributeID"].Value = productAttribute.AttributeID;
                    cmd.Parameters["@ProductID"].Value = productAttribute.ProductID;
                    cmd.Parameters["@AttributeValues"].Value = productAttribute.AttributeValues;
                    cmd.Parameters["@DisplayOrder"].Value = productAttribute.DisplayOrder;
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result++;
                    }
                }
                connection.Close();

            }
            return result;
        }

        public int Update(List<ProductAttribute> attributes)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE ProductAttributes
                                    SET (AttributeValues = @AttributeValues, DisplayOrder = @DisplayOrder)
                                    WHERE AttributeID = @AttributeID and ProductID = @ProductID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@AttributeID", SqlDbType.BigInt);
                cmd.Parameters.Add("@ProductID", SqlDbType.Int);
                cmd.Parameters.Add("@AttributeValues", SqlDbType.NVarChar);
                cmd.Parameters.Add("@DisplayOrder", SqlDbType.Int);
                foreach (ProductAttribute productAttribute in attributes)
                {
                    cmd.Parameters["@AttributeID"].Value = productAttribute.AttributeID;
                    cmd.Parameters["@ProductID"].Value = productAttribute.ProductID;
                    cmd.Parameters["@AttributeValues"].Value = productAttribute.AttributeValues;
                    cmd.Parameters["@DisplayOrder"].Value = productAttribute.DisplayOrder;
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result++;
                    }
                }

                connection.Close();
            }
            return result;
        }

        public int Delete(int[] productIDs)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE ProductAttributes
                                    WHERE ProductID = @ProductID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                foreach (int productID in productIDs)
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result++;
                    }
                }
                connection.Close();
            }
            return result;
        }
    }
}
