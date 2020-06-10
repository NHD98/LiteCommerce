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
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = " Create new Shipper";
                    Shipper shipper = new Shipper()
                    {
                        ShipperID = 0
                    };
                    return View(shipper);
                }
                else
                {
                    ViewBag.Title = "Edit a Shipper";
                    Shipper shipper = CatalogBLL.GetShipper(Convert.ToInt32(id));
                    if (shipper == null)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(shipper);
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ": " + ex.StackTrace);
            }
        }

        public ActionResult Input(Shipper shipper)
        {
            try
            {
                // Kiểm tra dữ liệu vào
                if (string.IsNullOrEmpty(shipper.CompanyName))
                {
                    ModelState.AddModelError("CompanyName", "Company Name is invalid");
                }
                if (string.IsNullOrEmpty(shipper.Phone))
                {
                    shipper.Phone = "";
                }
                if (ModelState.IsValid)
                {
                    // Lưu data vào DB
                    if (shipper.ShipperID == 0)
                    {
                        CatalogBLL.AddShipper(shipper);
                    }
                    else
                    {
                        CatalogBLL.UpdateShipper(shipper);
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ": " + ex.StackTrace);
            }
            return View(shipper);
        }
        [HttpPost]
        public ActionResult Delete(int[] shipperIDs)
        {
            if (shipperIDs != null)
            {
                CatalogBLL.DeleteShipper(shipperIDs);
            }
            return RedirectToAction("Index");
        }
    }
}