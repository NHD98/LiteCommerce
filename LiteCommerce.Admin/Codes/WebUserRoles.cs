using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin
{
    /// <summary>
    /// Định nghĩa danh sách các Role của user
    /// </summary>
    public class WebUserRoles
    {
        /// <summary>
        /// Không xác định
        /// </summary>
        public const string ANONYMOUS = "anonymous";
        /// <summary>
        /// nhân viên bán hàng
        /// </summary>
        public const string SALER = "saler";
        /// <summary>
        /// Nhân viên quản trị dữ liệu
        /// </summary>
        public const string DATA_MANAGER = "data_manager";
        /// <summary>
        /// Nhân viên quản trị tài khoản
        /// </summary>
        public const string ACCOUNT_MANAGER = "account_manager";
    }
}