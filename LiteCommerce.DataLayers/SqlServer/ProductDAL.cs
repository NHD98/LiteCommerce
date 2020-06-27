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
    public class ProductDAL : IProductDAL
    {
        private string connectionString;
        /// <summary>
        /// Hàm tạo, khởi tạo connectionString
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Product data)
        {
            int productID = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, Descriptions, PhotoPath)
                                    VALUES (@ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @Descriptions, @PhotoPath);
                                    SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Description);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);

                productID = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return productID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue, int supplierID, int categoryID)
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
                cmd.CommandText = @"select COUNT(*) from Products 
                                    where (@searchValue = N'' or ProductName like @searchValue)
                                           and (@supplierID = 0 or SupplierID = @supplierID)
                                           and (@categoryID = 0 or CategoryID = @categoryID)
                                    ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@supplierID", supplierID);
                cmd.Parameters.AddWithValue("@categoryID", categoryID);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductIDs"></param>
        /// <returns></returns>
        public int Delete(int[] ProductIDs)
        {

            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Products
                                            WHERE(ProductID = @ProductID) and ProductID not in (select Distinct ProductID from OrderDetails)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@ProductID", SqlDbType.Int);
                foreach (int productID in ProductIDs)
                {
                    cmd.Parameters["@ProductID"].Value = productID;
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
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public Product Get(int ProductID)
        {
            Product data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT Products.*, Categories.CategoryName, Suppliers.CompanyName as 'SupplierName' FROM Products 
                                    join Categories on Products.CategoryID = Categories.CategoryID 
                                    join Suppliers on Suppliers.SupplierID = Products.SupplierID
                                    WHERE ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductID", ProductID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Product()
                        {
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            Description = Convert.ToString(dbReader["Descriptions"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            ProductName = Convert.ToString(dbReader["ProductName"]),
                            QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                            SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                            UnitPrice = Convert.ToInt32(dbReader["UnitPrice"]),
                            CategoryName = Convert.ToString(dbReader["CategoryName"]),
                            SupplierName = Convert.ToString(dbReader["SupplierName"])
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
        /// <param name="categoryID"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public List<Product> List(int page, int pageSize, string searchValue, int categoryID, int supplierID)
        {
            List<Product> data = new List<Product>();
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
	                                select ROW_NUMBER() over(order by ProductName) as RowNumber, Products.*, Categories.CategoryName, Suppliers.CompanyName as 'SupplierName'
	                                from Products join Suppliers on Products.SupplierID = Suppliers.SupplierID join Categories on Categories.CategoryID = Products.CategoryID
	                                where ((@searchValue = N'') or (ProductName like @searchValue) )
	                                   and ((Products.SupplierID = @SupplierID) or (@SupplierID <= 0))
	                                   and ((Products.CategoryID = @categoryID) or (@categoryID <= 0))
	                                   ) as t
	                                    where (@pageSize <= 0) OR  (t.RowNumber between @pageSize * (@page -  1) + 1 and @page * @pageSize)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@categoryID", categoryID);
                cmd.Parameters.AddWithValue("@supplierID", supplierID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Product()
                        {
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            Description = Convert.ToString(dbReader["Descriptions"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            ProductName = Convert.ToString(dbReader["ProductName"]),
                            QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                            SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                            UnitPrice = Convert.ToInt32(dbReader["UnitPrice"]),
                            CategoryName = Convert.ToString(dbReader["CategoryName"]),
                            SupplierName = Convert.ToString(dbReader["SupplierName"])
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
        public bool Update(Product data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Products
                                           SET ProductName = @ProductName 
                                              ,SupplierID = @SupplierID
                                              ,CategoryID = @CategoryID
                                              ,QuantityPerUnit = @QuantityPerUnit
                                              ,UnitPrice = @UnitPrice
                                              ,Descriptions = @Descriptions
                                              ,PhotoPath = @PhotoPath
                                          WHERE ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Description);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
