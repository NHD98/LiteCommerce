using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IProductDAL
    {
        /// <summary>
        /// Hiển thị danh sách Product, có phân trang theo kích thước trang và có thể tìm kiếm
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        List<Product> List(int page, int pageSize, string searchValue, int categoryID, int supplierID);
        /// <summary>
        /// Đếm số lượng tìm kiếm được
        /// </summary>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        int Count(string searchValue, int supplierID, int categoryID);
        /// <summary>
        /// Lấy thông tin của 1 Product theo ID. Trả về Product lấy được.
        /// </summary>
        /// <param name="ProductID">ID của Product cần lấy thông tin</param>
        /// <returns></returns>
        Product Get(int ProductID);
        /// <summary>
        /// Nếu lỗi, hàm trả về giá trị nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data">Dữ liệu của Product cần bổ sung</param>
        /// <returns></returns>
        int Add(Product data);
        /// <summary>
        /// Cập nhật thông tin của 1 Product.
        /// Trả về true nếu cập nhật được, false nếu không cập nhật được.
        /// </summary>
        /// <param name="data">Thông tin đã sửa</param>
        /// <returns></returns>
        bool Update(Product data);
        /// <summary>
        /// Xóa các Product.
        /// Trả về số lượng Product đã được xóa
        /// </summary>
        /// <param name="ProductIDs">Danh sách các ID của các Product cần xóa</param>
        /// <returns></returns>
        int Delete(int[] ProductIDs);

    }
}
