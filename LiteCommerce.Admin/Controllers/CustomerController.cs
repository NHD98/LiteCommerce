﻿using LiteCommerce.BusinessLayers;
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
    [Authorize(Roles = WebUserRoles.DATA_MANAGER)]
    //[Authorize]
    public class CustomerController : Controller
    {
        // GET: Customer
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "", string country = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Customer> listOfCustomer = CatalogBLL.ListOfCustomers(page, pageSize, searchValue, country, out rowCount);

            var model = new Models.CustomerPaginationResult()
            {
                Data = listOfCustomer,
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Country = country
            };

            return View(model);
        }
        /// <summary>
        /// tao moi hoac sua
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = " Create new Customer";
                Customer customer = new Customer()
                {
                    CustomerID = ""
                };
                return View(customer);
            }
            else
            {
                ViewBag.Title = "Edit a Customer";
                Customer customer = CatalogBLL.GetCustomer(id);
                return View(customer);
            }
        }
        /// <summary>
        /// tao moi hoac sua
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Input(Customer model)
        {
            try
            {
                // kiểm tra dữ liệu vào
                if (string.IsNullOrEmpty(model.CustomerID) || model.CustomerID.Length != 5)
                {
                    ModelState.AddModelError("CustomerID", "CustomerID is invalid");
                }
                if (string.IsNullOrEmpty(model.Country))
                {
                    model.Country = "";
                }
                if (string.IsNullOrEmpty(model.ContactTitle))
                {
                    model.ContactTitle = "";
                }
                if (string.IsNullOrEmpty(model.ContactName))
                {
                    model.ContactName = "";
                }
                if (string.IsNullOrEmpty(model.CompanyName))
                {
                    model.CompanyName = "";
                }
                if (string.IsNullOrEmpty(model.City))
                {
                    model.City = "";
                }
                if (string.IsNullOrEmpty(model.Address))
                {
                    model.Address = "";
                }

                if (ModelState.IsValid)
                {
                    // Lưu dữ liệu vào DB
                    Customer existCustomer = CatalogBLL.GetCustomer(model.CustomerID);
                    if (existCustomer == null)
                    {
                        CatalogBLL.AddCustomer(model);
                    }
                    else
                    {
                        CatalogBLL.UpdateCustomer(model);
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ": " + ex.StackTrace);
            }
            return View(model);
        }
        /// <summary>
        /// xoa
        /// </summary>
        /// <param name="customerIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string[] customerIDs)
        {
            if (customerIDs != null)
            {
                CatalogBLL.DeleteCustomer(customerIDs);
            }
            return RedirectToAction("Index");
        }
    }
}