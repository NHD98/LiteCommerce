using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{/// <summary>
/// 
/// </summary>
    public class CategoryController : Controller
    {
        // GET: Category
        /// <summary>
        /// 
        /// </summary>
        [Authorize]
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Category> listOfCategory = CatalogBLL.ListOfCategories(page, pageSize, searchValue, out rowCount);

            var model = new Models.CategoryPaginationResult()
            {
                Data = listOfCategory,
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
                ViewBag.Title = " Create new Category";
            }
            else
            {
                ViewBag.Title = "Edit a Category";
            }
            return View();
        }
    }
}