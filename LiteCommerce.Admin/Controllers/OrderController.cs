using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = WebUserRoles.SALER)]
    //[Authorize]
    public class OrderController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string customerID = "", int employeeID = 0, int shipperID = 0)
        {
            int pageSize = 3;
            int rowCount = 0;

            List<Order> listOfOrders = OrderBLL.ListOfOrders(page, pageSize, customerID, employeeID, shipperID, out rowCount);
            var model = new Models.OrderPaginationResult()
            {
                Data = listOfOrders,
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                CustomerID = customerID,
                EmployeeID = employeeID,
                SearchValue = "",
                ShipperID = shipperID
            };
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = " Create new Order";
                Order order = new Order()
                {
                    OrderID = 0,
                    Details = new List<OrderDetail>()
                };
                return View(order);
            }
            else
            {
                ViewBag.Title = "Edit a Order";
                Order order = OrderBLL.Get(Convert.ToInt32(id));
                return View(order);
            }
        }
        [HttpPost]
        public ActionResult Create(Order order, int[] productIDs, int[] quantities)
        {
            //Kiểm tra hợp lệ dữ liệu
            if (order.OrderDate == new DateTime(0001, 01, 01))
            {
                ModelState.AddModelError("OrderDate", "OrderDate is invalid");
            }
            if (string.IsNullOrEmpty(order.CustomerID))
            {
                ModelState.AddModelError("CustomerID", "Please select a customer");
            }
            if (order.EmployeeID == 0)
            {
                ModelState.AddModelError("EmployeeID", "Please select an Employee");
            }
            if (order.RequiredDate == new DateTime(0001, 01, 01))
            {
                ModelState.AddModelError("RequiredDate", "RequiredDate is invalid");
            }
            if (string.IsNullOrEmpty(order.ShipAddress))
            {
                ModelState.AddModelError("ShipAddress", "Please enter ship address");
            }
            if (string.IsNullOrEmpty(order.ShipCity))
            {
                ModelState.AddModelError("ShipCity", "Please enter ship city");
            }
            if (string.IsNullOrEmpty(order.ShipCountry))
            {
                ModelState.AddModelError("ShipCountry", "Please enter ship country");
            }
            if (order.ShippedDate == new DateTime(0001, 01, 01))
            {
                ModelState.AddModelError("ShippedDate", "ShippedDate is invalid");
            }
            if (order.ShipperID == 0)
            {
                ModelState.AddModelError("ShipperID", "Please select Shipper");
            }
            if (productIDs != null)
            {
                try
                {
                    for (int i = 0; i < productIDs.Length; i++)
                    {
                        int temp = Convert.ToInt32(productIDs[i]);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Quantity", "Quantity(s) must be a number");
                }
            }

            if (ModelState.IsValid)
            {
                //Lưu vào DB
                if (order.OrderID == 0)
                {
                    //Tạo mới
                    int orderID = OrderBLL.AddOrder(order);
                    if (productIDs != null && quantities != null)
                    {
                        int detailLenght = productIDs.Length;
                        for (int i = 0; i < detailLenght; i++)
                        {
                            if (productIDs[i] <= 0 || quantities[i] <= 0)
                            {
                                continue;
                            }
                            else
                            {
                                OrderBLL.AddOrderDetail(new OrderDetail() { OrderID = orderID, ProductID = productIDs[i], Quantity = quantities[i] });
                            }
                        }
                    }
                }
                else
                {
                    //Sửa
                    OrderBLL.Update(order);
                    OrderBLL.DeleteOrderDetails(order.OrderID);
                    if (productIDs != null && quantities != null)
                    {
                        int detailLenght = productIDs.Length;
                        for (int i = 0; i < detailLenght; i++)
                        {
                            if (productIDs[i] <= 0 || quantities[i] <= 0)
                            {
                                continue;
                            }
                            else
                            {
                                OrderBLL.AddOrderDetail(new OrderDetail() { OrderID = order.OrderID, ProductID = productIDs[i], Quantity = quantities[i] });
                            }
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                order.Details = OrderBLL.ListOfOrderDetail(order.OrderID);
                if (order.Details == null)
                {
                    order.Details = new List<OrderDetail>();
                }
                return View(order);
            }
        }

        public ActionResult Delete(int[] orderIDs)
        {
            if (orderIDs.Length > 0)
            {
                for (int i = 0; i < orderIDs.Length; i++)
                {
                    OrderBLL.DeleteOrderDetails(orderIDs[i]);
                }
                OrderBLL.DeleteOrder(orderIDs);
            }
            return RedirectToAction("Index");
        }
    }
}