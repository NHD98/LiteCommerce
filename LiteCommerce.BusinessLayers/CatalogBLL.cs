using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    /// <summary>
    /// các chức năng nghiệp vụ liên quan đến quản lý dữ liệu chung của hệ thống như:
    /// nhà cung cấp, khác hàng, mặt hàng, ...
    /// </summary>
    public static class CatalogBLL
    {
        public static void Initialize(string connectionString)
        {
            SupplierDB = new DataLayers.SqlServer.SupplierDAL(connectionString);
        }
        #region Khai báo các thuộc tính giao tiếp với DAL
        /// <summary>
        /// 
        /// </summary>
        private static ISupplierDAL SupplierDB { get; set; }
        #endregion

        #region Khai báo các chức năng xử lý nghiệp vụ
        public static List<Supplier> ListOfSuppliers(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = SupplierDB.Count(searchValue);
            return SupplierDB.List(page, pageSize, searchValue);
        }
        #endregion
    }
}
