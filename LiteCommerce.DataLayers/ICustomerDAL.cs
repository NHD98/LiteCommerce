using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface ICustomerDAL
    {
        /// <summary>
        /// Hiển thị danh sách Customer, có phân trang theo kích thước trang và có thể tìm kiếm
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        List<Customer> List(int page, int pageSize, string searchValue, string country);
        /// <summary>
        /// Đếm số lượng tìm kiếm được
        /// </summary>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        int Count(string searchValue, string country);
        /// <summary>
        /// Lấy thông tin của 1 Customer theo ID. Trả về Customer lấy được.
        /// </summary>
        /// <param name="CustomerID">ID của Customer cần lấy thông tin</param>
        /// <returns></returns>
        Customer Get(string CustomerID);
        /// <summary>
        /// Bổ sung 1 Customer. Hàm trả về ID của Customer được bổ sung.
        /// Nếu lỗi, hàm trả về giá trị nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data">Dữ liệu của Customer cần bổ sung</param>
        /// <returns></returns>
        int Add(Customer data);
        /// <summary>
        /// Cập nhật thông tin của 1 Customer.
        /// Trả về true nếu cập nhật được, false nếu không cập nhật được.
        /// </summary>
        /// <param name="data">Thông tin đã sửa</param>
        /// <returns></returns>
        bool Update(Customer data);
        /// <summary>
        /// Xóa các Customer.
        /// Trả về số lượng Customer đã được xóa
        /// </summary>
        /// <param name="CustomerIDs">Danh sách các ID của các Customer cần xóa</param>
        /// <returns></returns>
        int Delete(string[] CustomerIDs);
    }
}
