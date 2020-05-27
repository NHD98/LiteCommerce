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
    [Authorize]
    public class ShipperController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Shipper> listOfShipper = CatalogBLL.ListOfShippers(page, pageSize, searchValue, out rowCount);

            var model = new Models.ShipperPaginationResult()
            {
                Page = page,
                Data = listOfShipper,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue
            };
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = " Create new Shipper";
            }
            else
            {
                ViewBag.Title = "Edit a Shipper";
            }
            return View();
        }
    }
}