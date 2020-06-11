using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class CustomerUserAccountDAL : IUserAccountDAL
    {
        private string connectionString;
        public CustomerUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserAccount Authorize(string userName, string password)
        {
            //TODO: Kiểm tra thông tin tài khoản từ bảng Customer
            return new UserAccount() { UserID = userName, FullName = "Khách Hàng", Photo = "kh.jpg" };
        }
    }
}
