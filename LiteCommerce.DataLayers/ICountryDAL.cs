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
        List<Country> List(int page, int pageSize, string searchValue);
        int Count(string searchValue);
        Country Get(string countryID);
        bool Add(Country country);
        bool Update(Country country);
        int Delete(string[] countryIDs);
    }
}
