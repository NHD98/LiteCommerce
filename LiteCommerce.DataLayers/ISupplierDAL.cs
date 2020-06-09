using LiteCommerce.DomainModels;
using System.Collections.Generic;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Interface Supplier
    /// </summary>
    public interface ISupplierDAL
    {
        /// <summary>
        /// Hiển thị danh sách Supplier, có phân trang theo kích thước trang và có thể tìm kiếm
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        List<Supplier> List(int page, int pageSize, string searchValue);

        List<Supplier> ListAll();
        /// <summary>
        /// Đếm số lượng tìm kiếm được
        /// </summary>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy thông tin của 1 Supplier theo ID. Trả về Supplier lấy được.
        /// </summary>
        /// <param name="supplierID">ID của Supplier cần lấy thông tin</param>
        /// <returns></returns>
        Supplier Get(int supplierID);
        /// <summary>
        /// Bổ sung 1 Supplier. Hàm trả về ID của Supplier được bổ sung.
        /// Nếu lỗi, hàm trả về giá trị nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data">Dữ liệu của Supplier cần bổ sung</param>
        /// <returns></returns>
        int Add(Supplier data);
        /// <summary>
        /// Cập nhật thông tin của 1 Supplier.
        /// Trả về true nếu cập nhật được, false nếu không cập nhật được.
        /// </summary>
        /// <param name="data">Thông tin đã sửa</param>
        /// <returns></returns>
        bool Update(Supplier data);
        /// <summary>
        /// Xóa các Supplier.
        /// Trả về số lượng Supplier đã được xóa
        /// </summary>
        /// <param name="supplierIDs">Danh sách các ID của các Supplier cần xóa</param>
        /// <returns></returns>
        int Delete(int[] supplierIDs);
    }
}
