using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    /// <summary>
    /// các chức năng nghiệp vụ liên quan đến quản lý dữ liệu chung của hệ thống như:
    /// nhà cung cấp, khác hàng, mặt hàng, ...
    /// </summary>
    public static class CatalogBLL
    {
        public static void Initialize(string connectionString)
        {
            SupplierDB = new SupplierDAL(connectionString);
            CustomerDB = new CustomerDAL(connectionString);
            ShipperDB = new ShipperDAL(connectionString);
            CategoryDB = new CategoryDAL(connectionString);
            ProductDB = new ProductDAL(connectionString);
            ProductAttributeDB = new ProductAttributeDAL(connectionString);
            AttributeDB = new AttributeDAL(connectionString);
            CountryDB = new CountryDAL(connectionString);
        }
        #region Khai báo các thuộc tính giao tiếp với DAL
        /// <summary>
        /// 
        /// </summary>
        private static ISupplierDAL SupplierDB { get; set; }
        private static ICustomerDAL CustomerDB { get; set; }
        private static IShipperDAL ShipperDB { get; set; }
        private static ICategoryDAL CategoryDB { get; set; }
        private static IProductDAL ProductDB { get; set; }
        private static IProductAttributeDAL ProductAttributeDB { get; set; }
        private static IAttributeDAL AttributeDB { get; set; }
        private static ICountryDAL CountryDB { get; set; }
        #endregion

        #region Khai báo các chức năng xử lý nghiệp vụ
        /// <summary>
        /// Lấy danh sách các Supplier. Có phân trang.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            //if (pageSize <= 0)
            //    pageSize = 20;
            rowCount = SupplierDB.Count(searchValue);
            return SupplierDB.List(page, pageSize, searchValue);
        }

        public static List<Supplier> ListOfAllSuppliers()
        {
            return SupplierDB.ListAll();
        }
        /// <summary>
        /// Lấy thông tin của 1 supplier dựa vào ID
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static Supplier GetSupplier(int supplierID)
        {
            return SupplierDB.Get(supplierID);
        }
        /// <summary>
        /// Thêm 1 supplier. Hàm trả về ID của supplier được thêm vào.
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public static int AddSupplier(Supplier supplier)
        {
            return SupplierDB.Add(supplier);
        }
        /// <summary>
        /// Cập nhật 1 supplier. Hàm trả về true nếu cập nhật thành công, false nếu thất bại.
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier supplier)
        {
            return SupplierDB.Update(supplier);
        }
        /// <summary>
        /// Xóa các supplier dựa vào ID
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        public static int DeleteSuppliers(int[] supplierIDs)
        {
            return SupplierDB.Delete(supplierIDs);
        }
        /// <summary>
        /// Lây danh sách các Customer. Có phân trang.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(int page, int pageSize, string searchValue, string country, out int rowCount)
        {
            if (page < 1)
                page = 1;
            rowCount = CustomerDB.Count(searchValue, country);
            return CustomerDB.List(page, pageSize, searchValue, country);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer customer)
        {
            return CustomerDB.Add(customer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static Customer GetCustomer(string customerID)
        {
            return CustomerDB.Get(customerID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer customer)
        {
            return CustomerDB.Update(customer);
        }
        /// <summary>
        /// xoa khach hang
        /// </summary>
        /// <param name="customerIDs"></param>
        /// <returns></returns>
        public static int DeleteCustomer(string[] customerIDs)
        {
            return CustomerDB.Delete(customerIDs);
        }
        /// <summary>
        /// Lấy danh sách các Shipper. Có phân trang.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize <= 0)
                pageSize = 20;
            rowCount = ShipperDB.Count(searchValue);
            return ShipperDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipper"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper shipper)
        {
            return ShipperDB.Add(shipper);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipper"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper shipper)
        {
            return ShipperDB.Update(shipper);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperIDs"></param>
        /// <returns></returns>
        public static int DeleteShipper(int[] shipperIDs)
        {
            return ShipperDB.Delete(shipperIDs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static Shipper GetShipper(int shipperID)
        {
            return ShipperDB.Get(shipperID);
        }
        /// <summary>
        /// Lấy danh sách Category. Có phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            //if (pageSize <= 0)
            //{
            //    pageSize = 20;
            //}
            rowCount = CategoryDB.Count(searchValue);
            return CategoryDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static Category GetCategory(int categoryID)
        {
            return CategoryDB.Get(categoryID);
        }
        /// <summary>
        /// Thêmm mới 1 category, hàm trả về ID của category vừa thêm được, nếu bằng 0 nghĩa là thêm thất bại
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static int AddCategory(Category category)
        {
            return CategoryDB.Add(category);
        }
        /// <summary>
        /// Cập nhật 1 category. Hàm trả về true nếu thành công, false nếu thất bại
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category category)
        {
            return CategoryDB.Update(category);
        }
        /// <summary>
        /// Xóa các categories, hàm trả về số lượng categories xóa được
        /// </summary>
        /// <param name="categoryIDs"></param>
        /// <returns></returns>
        public static int DeleteCategories(int[] categoryIDs)
        {
            return CategoryDB.Delete(categoryIDs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryID"></param>
        /// <param name="supplierID"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Product> ListOfProduct(int page, int pageSize, string searchValue, int categoryID, int supplierID, out int rowCount)
        {
            rowCount = ProductDB.Count(searchValue, supplierID, categoryID);
            return ProductDB.List(page, pageSize, searchValue, categoryID, supplierID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static Product GetProduct(int productID)
        {
            return ProductDB.Get(productID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static int AddProduct(Product product)
        {
            return ProductDB.Add(product);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product product)
        {
            return ProductDB.Update(product);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productIDs"></param>
        /// <returns></returns>
        public static int DeleteProduct(int[] productIDs)
        {
            ProductAttributeDB.Delete(productIDs);
            return ProductDB.Delete(productIDs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static int AddProductAttribute(List<ProductAttribute> attributes)
        {
            return ProductAttributeDB.Add(attributes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static int UpdateProductAttribute(List<ProductAttribute> attributes)
        {
            return ProductAttributeDB.Update(attributes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static List<ProductAttribute> GetProductAttributes(int productID)
        {
            return ProductAttributeDB.Get(productID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<DomainModels.Attribute> GetAttributes()
        {
            return AttributeDB.List();
        }
        /// <summary>
        /// lay danh sach quoc gia
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Country> GetCountries(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = CountryDB.Count(searchValue);
            return CountryDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        /// lay chi tiet quoc gia
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        public static Country GetCountry(string countryID)
        {
            return CountryDB.Get(countryID);
        }
        /// <summary>
        /// them quoc gia
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public static bool AddCountry(Country country)
        {
            return CountryDB.Add(country);
        }
        /// <summary>
        /// sua quoc gia
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public static bool UpdateCountry(Country country)
        {
            return CountryDB.Update(country);
        }
        /// <summary>
        /// xoa quoc gia
        /// </summary>
        /// <param name="countryIDs"></param>
        /// <returns></returns>
        public static int DeleteCountries(string[] countryIDs)
        {
            return CountryDB.Delete(countryIDs);
        }
        #endregion
    }
}
