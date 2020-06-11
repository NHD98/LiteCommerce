using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// Lưu thông tin liên quan đến tài khoản đăng nhập hệ thống
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// ID của tài khoản đăng nhập vào hệ thống
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// Tên đầy đủ của User (FirstName và LastName)
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Tên file ảnh của User
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// Chức danh của User
        /// </summary>
        public string Title { get; set; }
    }
}
