using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    public class CountryController : Controller
    {
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Country> listOfCountry = CatalogBLL.GetCountries(page, pageSize, searchValue, out rowCount);

            var model = new Models.CountryPaginationResult()
            {
                Page = page,
                Data = listOfCountry,
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
                ViewBag.Title = "Create Country";
                Country country = new Country() { CountryID = "", CountryName = "" };
                return View(country);
            }
            else
            {
                ViewBag.Title = "Edit Country";
                Country country = CatalogBLL.GetCountry(id);
                return View(country);
            }
        }
        [HttpPost]
        public ActionResult Input(Country country)
        {
            Country existCountry = CatalogBLL.GetCountry(country.CountryID);
            if (existCountry == null)
            {
                CatalogBLL.AddCountry(country);
            }
            else
            {
                CatalogBLL.UpdateCountry(country);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(string[] countryIDs)
        {
            if (countryIDs.Length > 0)
            {
                CatalogBLL.DeleteCountries(countryIDs);
            }
            return RedirectToAction("Index");
        }
    }
}