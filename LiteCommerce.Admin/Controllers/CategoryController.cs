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
    [Authorize(Roles = WebUserRoles.DATA_MANAGER)]
    //[Authorize]
    public class CategoryController : Controller
    {
        // GET: Category
        /// <summary>
        /// 
        /// </summary>
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
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = " Create new Category";
                    Category newCategory = new Category()
                    {
                        CategoryID = 0
                    };
                    return View(newCategory);
                }
                else
                {
                    ViewBag.Title = "Edit a Category";
                    Category editCategory = CatalogBLL.GetCategory(Convert.ToInt32(id));
                    if (editCategory == null)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(editCategory);
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ": " + ex.StackTrace);
            }
        }

        public ActionResult Input(Category model)
        {
            try
            {
                //TODO: Kiểm tra tính hợp lệ của dữ liệu được nhập
                if (string.IsNullOrEmpty(model.CategoryName))
                {
                    ModelState.AddModelError("CategoryName", "Category Name is invalid");
                }
                if (string.IsNullOrEmpty(model.Description))
                {
                    ModelState.AddModelError("Description", "Description is invalid");
                }

                if (ModelState.IsValid)
                {
                    //TODO: Lưu dữ liệu vào DB
                    if (model.CategoryID == 0)
                    {
                        CatalogBLL.AddCategory(model);
                    }
                    else
                    {
                        CatalogBLL.UpdateCategory(model);
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

        [HttpPost]
        public ActionResult Delete(int[] categoryIDs = null)
        {
            if (categoryIDs != null)
            {
                CatalogBLL.DeleteCategories(categoryIDs);
            }
            return RedirectToAction("Index");
        }
    }
}