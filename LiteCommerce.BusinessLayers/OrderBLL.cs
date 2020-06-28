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
    public static class OrderBLL
    {
        /// <summary>
        /// khoi tao
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            OrderDB = new OrderDAL(connectionString);
        }

        #region Khai báo các thuộc tính giao tiếp với DAL
        private static IOrderDAL OrderDB { get; set; }
        #endregion
        /// <summary>
        /// danh sach order
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="customerID"></param>
        /// <param name="employeeID"></param>
        /// <param name="shipperID"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Order> ListOfOrders(int page, int pageSize, string customerID, int employeeID, int shipperID, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize <= 0)
                pageSize = 3;
            rowCount = OrderDB.Count(customerID, employeeID, shipperID);
            return OrderDB.List(page, pageSize, customerID, employeeID, shipperID);
        }
        /// <summary>
        /// them order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static int AddOrder(Order order)
        {
            return OrderDB.Add(order);
        }
        /// <summary>
        /// xoa orders
        /// </summary>
        /// <param name="orderIDs"></param>
        /// <returns></returns>
        public static int DeleteOrder(int[] orderIDs)
        {
            return OrderDB.Delete(orderIDs);
        }
        /// <summary>
        /// lay chi tiet order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static Order Get(int orderID)
        {
            return OrderDB.Get(orderID);
        }
        /// <summary>
        /// cap nhat order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static bool Update(Order order)
        {
            return OrderDB.Update(order);
        }
        /// <summary>
        /// xoa chi tiet order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static int DeleteOrderDetails(int orderID)
        {
            return OrderDB.DeleteDetails(orderID);
        }
        /// <summary>
        /// them chi tiet order
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static int AddOrderDetail(OrderDetail detail)
        {
            return OrderDB.AddDetail(detail);
        }
        /// <summary>
        /// danh sach chi tiet order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static List<OrderDetail> ListOfOrderDetail(int orderID)
        {
            return OrderDB.ListOfDetails(orderID);
        }
    }
}
