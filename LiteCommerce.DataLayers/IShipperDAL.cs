using LiteCommerce.DomainModels;
using System.Collections.Generic;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Interface Shipper
    /// </summary>
    public interface IShipperDAL
    { /// <summary>
      /// Hiển thị danh sách Shipper, có phân trang theo kích thước trang và có thể tìm kiếm
      /// </summary>
      /// <param name="page">Số trang</param>
      /// <param name="pageSize">Kích thước trang</param>
      /// <param name="searchValue">Từ khóa tìm kiếm</param>
      /// <returns></returns>
        List<Shipper> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// Đếm số lượng tìm kiếm được
        /// </summary>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy thông tin của 1 Shipper theo ID. Trả về Shipper lấy được.
        /// </summary>
        /// <param name="ShipperID">ID của Shipper cần lấy thông tin</param>
        /// <returns></returns>
        Shipper Get(int ShipperID);
        /// <summary>
        /// Bổ sung 1 Shipper. Hàm trả về ID của Shipper được bổ sung.
        /// Nếu lỗi, hàm trả về giá trị nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data">Dữ liệu của Shipper cần bổ sung</param>
        /// <returns></returns>
        int Add(Shipper data);
        /// <summary>
        /// Cập nhật thông tin của 1 Shipper.
        /// Trả về true nếu cập nhật được, false nếu không cập nhật được.
        /// </summary>
        /// <param name="data">Thông tin đã sửa</param>
        /// <returns></returns>
        bool Update(Shipper data);
        /// <summary>
        /// Xóa các Shipper.
        /// Trả về số lượng Shipper đã được xóa
        /// </summary>
        /// <param name="ShipperIDs">Danh sách các ID của các Shipper cần xóa</param>
        /// <returns></returns>
        int Delete(int[] ShipperIDs);
    }
}
