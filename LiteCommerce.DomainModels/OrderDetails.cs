using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// Chi tiết đơn hàng
    /// </summary>
    public class OrderDetails
    {
        public int OrderID { get; set; }
        public List<Product> Products { get; set; }
    }
}
