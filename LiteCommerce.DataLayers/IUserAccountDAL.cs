using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserAccountDAL
    {
        /// <summary>
        /// Kiểm tra username và password có hợp lệ hay không
        /// nếu hợp lệ, trả về thông tin của User,
        /// ngược lại thì trả về giá trị null
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserAccount Authorize(string userName, string password);
    }
}
