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
    public class CategoryDAL : ICategoryDAL
    {
        private string connectionString;

        public CategoryDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// them category
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Category data)
        {
            int categoryID = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Categories
                                          (
	                                         CategoryName,
                                             Description
                                          )
                                          VALUES
                                          (
	                                          @CategoryName, @Description
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("CategoryName", data.CategoryName);
                cmd.Parameters.AddWithValue("Description", data.Description);
                categoryID = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return categoryID;
        }
        /// <summary>
        /// dem category
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
                cmd.CommandText = "select COUNT(*) from Categories where @searchValue = N'' or CategoryName like @searchValue";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return count;
        }
        /// <summary>
        /// xoa category
        /// </summary>
        /// <param name="CategoryIDs"></param>
        /// <returns></returns>
        public int Delete(int[] categoryIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Categories
                                            WHERE(CategoryID = @categoryID)
                                              AND(CategoryID NOT IN(SELECT CategoryID FROM Products))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@categoryID", SqlDbType.Int);
                foreach (int categoryID in categoryIDs)
                {
                    cmd.Parameters["@categoryID"].Value = categoryID;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }
        /// <summary>
        /// lay chi tiet category
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public Category Get(int categoryID)
        {
            Category data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Categories WHERE CategoryID = @categoryID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@categoryID", categoryID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Category()
                        {
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            CategoryName = Convert.ToString(dbReader["CategoryName"]),
                            Description = Convert.ToString(dbReader["Description"])
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// danh sach cac category
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public List<Category> List(int page, int pageSize, string searchValue)
        {

            List<Category> data = new List<Category>();
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
	                                    select ROW_NUMBER() over(order by CategoryName) as RowNumber, Categories.*
	                                    from Categories
	                                    where (@searchValue = N'') or (CategoryName like @searchValue)
                                    ) as t
                                    where (@pageSize <= 0) or
                                        (t.RowNumber between @pageSize * (@page -  1) + 1 and @page * @pageSize)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Category()
                        {
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            CategoryName = Convert.ToString(dbReader["CategoryName"]),
                            Description = Convert.ToString(dbReader["Description"])
                        });
                    }
                }

                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// cap nhat category
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Category data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Categories
                                           SET CategoryName = @categoryName,
                                               Description = @description
                                          WHERE CategoryID = @categoryID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@categoryName", data.CategoryName);
                cmd.Parameters.AddWithValue("@description", data.Description);
                cmd.Parameters.AddWithValue("@categoryID", data.CategoryID);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return rowsAffected > 0;
        }
    }
}
