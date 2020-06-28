using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface ICountryDAL
    {
        /// <summary>
        /// danh sach quoc gia
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Country> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// dem quoc gia
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// lay chi tiet quoc gia
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        Country Get(string countryID);
        /// <summary>
        /// them quoc gia
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        bool Add(Country country);
        /// <summary>
        /// cap nhat quoc gia
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        bool Update(Country country);
        /// <summary>
        /// xoa quoc gia
        /// </summary>
        /// <param name="countryIDs"></param>
        /// <returns></returns>
        int Delete(string[] countryIDs);
    }
}
