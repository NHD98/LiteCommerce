using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public class CustomerPaginationResult : PaginationResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Customer> Data { get; set; }
    }
}