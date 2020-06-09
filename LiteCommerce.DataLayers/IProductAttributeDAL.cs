using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IProductAttributeDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        int Add(List<ProductAttribute> attributes);

        int Update(List<ProductAttribute> attributes);

        int Delete(int[] productIDs);
    }
}
