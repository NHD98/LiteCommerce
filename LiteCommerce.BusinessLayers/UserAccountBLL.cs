using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    /// <summary>
    /// Cung cấp chức năng tác nghiệp liên quan đến tài khoản
    /// </summary>
    public class UserAccountBLL
    {
        private static string _connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static UserAccount Authorize(string userName, string password, UserAccountTypes userType)
        {
            IUserAccountDAL userAccountDB;
            switch (userType)
            {
                case UserAccountTypes.Employee:
                    userAccountDB = new EmployeeUserAccountDAL(_connectionString);
                    break;
                case UserAccountTypes.Customer:
                    userAccountDB = new CustomerUserAccountDAL(_connectionString);
                    break;
                default:
                    return null;
            }
            return userAccountDB.Authorize(userName, password);
        }
    }
}
