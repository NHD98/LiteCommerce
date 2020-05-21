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
    public class OrderController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
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
            }
            else
            {
                ViewBag.Title = "Edit a Order";
            }
            return View();
        }
    }
}