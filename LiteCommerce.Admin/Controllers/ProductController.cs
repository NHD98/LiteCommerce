using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ProductController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "", int categoryID = 0, int supplierID = 0)
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Product> listOfProduct = CatalogBLL.ListOfProduct(page, pageSize, searchValue, categoryID, supplierID, out rowCount);

            var model = new Models.ProductPaginationResult()
            {
                Page = page,
                Data = listOfProduct,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Category = categoryID.ToString(),
                Supplier = supplierID.ToString()
            };

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = " Create new Product";
                    Product newProduct = new Product()
                    {
                        ProductID = 0
                    };
                    return View(newProduct);
                }
                else
                {
                    ViewBag.Title = "Edit a Product";
                    int productID = Convert.ToInt32(id);
                    Product editProduct = CatalogBLL.GetProduct(productID);
                    List<ProductAttribute> atts = CatalogBLL.GetProductAttributes(productID);
                    editProduct.Attributes = atts;
                    return View(editProduct);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ": " + ex.StackTrace);
            }
        }

        public ActionResult Input(Product product, string[] attributes, HttpPostedFileBase uploadFile)
        {
            try
            {
                if (product.CategoryID == 0)
                {
                    ModelState.AddModelError("CategoryID", "Please select a Category");
                }
                if (product.SupplierID == 0)
                {
                    ModelState.AddModelError("SupplierID", "Please select a Supplier");
                }
                if (product.Description == null)
                {
                    product.Description = "";
                }
                if (string.IsNullOrEmpty(product.PhotoPath))
                {
                    Product existProduct = CatalogBLL.GetProduct(product.ProductID);
                    if (existProduct != null)
                    {
                        product.PhotoPath = existProduct.PhotoPath;
                    }
                    else
                    {
                        product.PhotoPath = "";
                    }
                }
                if (string.IsNullOrEmpty(product.ProductName))
                {
                    ModelState.AddModelError("ProductName", "Product Name is invalid");
                }
                if (string.IsNullOrEmpty(product.QuantityPerUnit))
                {
                    ModelState.AddModelError("QuantityPerUnit", "Quantity Per Unit is invalid");
                }
                if (product.UnitPrice == 0)
                {
                    ModelState.AddModelError("UnitPrice", "Unit Price is invalid");
                }

                //Upload anh
                if (uploadFile != null)
                {
                    string _FileName = Path.GetFileName(uploadFile.FileName);
                    _FileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(_FileName);
                    string _path = Path.Combine(Server.MapPath("~/Uploads/Images"), _FileName);
                    uploadFile.SaveAs(_path);
                    product.PhotoPath = _FileName;
                }
                if (ModelState.IsValid)
                {
                    //Luu vao DB
                    if (product.ProductID == 0)
                    {
                        int productID = CatalogBLL.AddProduct(product);
                        if (attributes != null)
                        {
                            List<ProductAttribute> productAttributes = new List<ProductAttribute>();
                            int attLen = attributes.Count();
                            for (int i = 0; i < attLen; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        productAttributes.Add(new ProductAttribute() { AttributeID = 1, AttributeName = "Size", AttributeValues = attributes[i], DisplayOrder = 1, ProductID = productID });
                                        break;
                                    case 1:
                                        productAttributes.Add(new ProductAttribute() { AttributeID = 2, AttributeName = "Color", AttributeValues = attributes[i], DisplayOrder = 1, ProductID = productID });
                                        break;
                                    case 2:
                                        productAttributes.Add(new ProductAttribute() { AttributeID = 3, AttributeName = "Material", AttributeValues = attributes[i], DisplayOrder = 1, ProductID = productID });
                                        break;
                                    case 3:
                                        productAttributes.Add(new ProductAttribute() { AttributeID = 4, AttributeName = "Origin", AttributeValues = attributes[i], DisplayOrder = 1, ProductID = productID });
                                        break;
                                    default:
                                        break;
                                }
                            }
                            CatalogBLL.AddProductAttribute(productAttributes);
                        }
                    }
                    else
                    {
                        CatalogBLL.UpdateProduct(product);
                        if (attributes != null)
                        {
                            List<ProductAttribute> productAttributes = new List<ProductAttribute>();
                            int attLen = attributes.Length;
                            for (int i = 0; i < attLen; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        productAttributes.Add(new ProductAttribute() { AttributeID = 1, AttributeName = "Size", AttributeValues = attributes[i], DisplayOrder = 1, ProductID = product.ProductID });
                                        break;
                                    case 1:
                                        productAttributes.Add(new ProductAttribute() { AttributeID = 2, AttributeName = "Color", AttributeValues = attributes[i], DisplayOrder = 1, ProductID = product.ProductID });
                                        break;
                                    case 2:
                                        productAttributes.Add(new ProductAttribute() { AttributeID = 3, AttributeName = "Material", AttributeValues = attributes[i], DisplayOrder = 1, ProductID = product.ProductID });
                                        break;
                                    case 3:
                                        productAttributes.Add(new ProductAttribute() { AttributeID = 4, AttributeName = "Origin", AttributeValues = attributes[i], DisplayOrder = 1, ProductID = product.ProductID });
                                        break;
                                    default:
                                        break;
                                }
                            }
                            CatalogBLL.UpdateProductAttribute(productAttributes);
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ": " + ex.StackTrace);
            }
            return View(product);
        }

        public ActionResult Delete(int[] productIDs)
        {
            if (productIDs != null)
            {
                CatalogBLL.DeleteProduct(productIDs);
            }
            return RedirectToAction("Index");
        }
    }
}