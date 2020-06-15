using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IEmployeeDAL
    {
        /// <summary>
        /// Hiển thị danh sách Employee, có phân trang theo kích thước trang và có thể tìm kiếm
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        List<Employee> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// Đếm số lượng tìm kiếm được
        /// </summary>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy thông tin của 1 Employee theo ID. Trả về Employee lấy được.
        /// </summary>
        /// <param name="EmployeeID">ID của Employee cần lấy thông tin</param>
        /// <returns></returns>
        Employee Get(int EmployeeID);
        /// <summary>
        /// Bổ sung 1 Employee. Hàm trả về ID của Employee được bổ sung.
        /// Nếu lỗi, hàm trả về giá trị nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data">Dữ liệu của Employee cần bổ sung</param>
        /// <returns></returns>
        int Add(Employee data);
        /// <summary>
        /// Cập nhật thông tin của 1 Employee.
        /// Trả về true nếu cập nhật được, false nếu không cập nhật được.
        /// </summary>
        /// <param name="data">Thông tin đã sửa</param>
        /// <returns></returns>
        bool Update(Employee data);
        /// <summary>
        /// Xóa các Employee.
        /// Trả về số lượng Employee đã được xóa
        /// </summary>
        /// <param name="EmployeeIDs">Danh sách các ID của các Employee cần xóa</param>
        /// <returns></returns>
        int Delete(int[] EmployeeIDs);
        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ChangePassword(int userID, string password);
    }
}
