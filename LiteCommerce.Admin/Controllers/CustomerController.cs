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
    public class CustomerController : Controller
    {
        // GET: Customer
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Customer> listOfCustomer = CatalogBLL.ListOfCustomers(page, pageSize, searchValue, out rowCount);

            var model = new Models.CustomerPaginationResult()
            {
                Data = listOfCustomer,
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue
            };

            return View(model);
        }
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = " Create new Customer";
            }
            else
            {
                ViewBag.Title = "Edit a Customer";
            }
            return View();
        }
    }
}