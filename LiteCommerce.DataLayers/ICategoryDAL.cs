using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface ICategoryDAL
    {
        /// <summary>
        /// Hiển thị danh sách Category, có phân trang theo kích thước trang và có thể tìm kiếm
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        List<Category> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// Đếm số lượng tìm kiếm được
        /// </summary>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy thông tin của 1 Category theo ID. Trả về Category lấy được.
        /// </summary>
        /// <param name="CategoryID">ID của Category cần lấy thông tin</param>
        /// <returns></returns>
        Category Get(int CategoryID);
        /// <summary>
        /// Bổ sung 1 Category. Hàm trả về ID của Category được bổ sung.
        /// Nếu lỗi, hàm trả về giá trị nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data">Dữ liệu của Category cần bổ sung</param>
        /// <returns></returns>
        int Add(Category data);
        /// <summary>
        /// Cập nhật thông tin của 1 Category.
        /// Trả về true nếu cập nhật được, false nếu không cập nhật được.
        /// </summary>
        /// <param name="data">Thông tin đã sửa</param>
        /// <returns></returns>
        bool Update(Category data);
        /// <summary>
        /// Xóa các Category.
        /// Trả về số lượng Category đã được xóa
        /// </summary>
        /// <param name="CategoryIDs">Danh sách các ID của các Category cần xóa</param>
        /// <returns></returns>
        int Delete(int[] CategoryIDs);
    }
}
