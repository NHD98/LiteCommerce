﻿using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin
{
    public static class SelectListHelper
    {
        /// <summary>
        /// Select List các quốc gia.
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Countries()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (Country country in CatalogBLL.GetCountries())
            {
                list.Add(new SelectListItem() { Value = country.CountryID.Trim(), Text = country.CountryName.Trim() });
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Titles()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "Sales Representative", Text = "Sales Representative" });
            list.Add(new SelectListItem() { Value = "Vice President, Sales", Text = "Vice President, Sales" });
            list.Add(new SelectListItem() { Value = "Sales Manager", Text = "Sales Manager" });
            list.Add(new SelectListItem() { Value = "Inside Sales Coordinator", Text = "Inside Sales Coordinator" });
            return list;
        }

        public static List<SelectListItem> Categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            int count;
            CatalogBLL.ListOfCategories(1, -1, "", out count).ForEach(category =>
            {
                list.Add(new SelectListItem()
                {
                    Value = category.CategoryID.ToString(),
                    Text = category.CategoryName
                });
            });

            return list;
        }

        public static List<SelectListItem> Suppliers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            int count;
            CatalogBLL.ListOfSuppliers(1, -1, "", out count).ForEach(supplier =>
            {
                list.Add(new SelectListItem()
                {
                    Value = supplier.SupplierID.ToString(),
                    Text = supplier.CompanyName
                });
            });

            return list;
        }

        public static List<ProductAttribute> Attributes(int productID)
        {
            if (productID > 0)
            {
                return CatalogBLL.GetProductAttributes(productID);
            }
            else
            {
                List<ProductAttribute> list = new List<ProductAttribute>();
                List<DomainModels.Attribute> atts = CatalogBLL.GetAttributes();
                foreach (DomainModels.Attribute att in atts)
                {
                    list.Add(new ProductAttribute() { AttributeID = att.AttributeID, AttributeName = att.AttributeName, AttributeValues = "", DisplayOrder = 1, ProductID = 0 });
                }
                return list;
            }
        }
    }
}