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
        /// <summary>
        /// them thuoc tinh
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
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
        /// <summary>
        /// cap nhat thuoc tinh
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public int Update(List<ProductAttribute> attributes)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE ProductAttributes
                                    SET AttributeValues = @AttributeValues, DisplayOrder = @DisplayOrder
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
        /// <summary>
        /// xoa thuoc tinh
        /// </summary>
        /// <param name="productIDs"></param>
        /// <returns></returns>
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
                cmd.Parameters.Add("@ProductID", SqlDbType.Int);
                foreach (int productID in productIDs)
                {
                    cmd.Parameters["@ProductID"].Value = productID;
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result++;
                    }
                }
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// lay chi tiet san pham
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public List<ProductAttribute> Get(int productID)
        {
            List<ProductAttribute> list = new List<ProductAttribute>();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT p.* , a.AttributeName
                                    FROM ProductAttributes as p 
                                    join Attributes as a on p.AttributeID = a.AttributeID  
                                    WHERE ProductID = @ProductID";
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        list.Add(new ProductAttribute()
                        {
                            AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                            AttributeName = Convert.ToString(dbReader["AttributeName"]).Trim(),
                            AttributeValues = Convert.ToString(dbReader["AttributeValues"]).Trim(),
                            DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"]),
                            ProductID = Convert.ToInt32(dbReader["ProductID"])
                        });
                    }
                }
            }
            return list;
        }
    }
}
