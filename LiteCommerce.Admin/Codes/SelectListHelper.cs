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
            list.Add(new SelectListItem() { Value = "USA", Text = "United States" });
            list.Add(new SelectListItem() { Value = "UK", Text = "England" });
            list.Add(new SelectListItem() { Value = "VN", Text = "Vietnam" });
            return list;
        }
    }
}