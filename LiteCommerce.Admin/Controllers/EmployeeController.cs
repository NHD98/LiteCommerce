﻿using LiteCommerce.BusinessLayers;
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
    [Authorize(Roles = WebUserRoles.ACCOUNT_MANAGER)]
    //[Authorize]
    public class EmployeeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Employee> listOfEmployee = EmployeeBLL.ListOfEmployees(page, pageSize, searchValue, out rowCount);

            var model = new Models.EmployeePaginationResult()
            {
                Data = listOfEmployee,
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue
            };

            return View(model);
        }
        /// <summary>
        /// tao moi hoac sua
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
                    ViewBag.Title = " Create new Employee";
                    Employee employee = new Employee()
                    {
                        EmployeeID = 0
                    };
                    return View(employee);
                }
                else
                {
                    ViewBag.Title = "Edit a Employee";
                    Employee employee = EmployeeBLL.GetEmployee(Convert.ToInt32(id));
                    if (employee == null)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ": " + ex.StackTrace);
            }
        }
        /// <summary>
        /// tao moi hoac sua
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Input(Employee model, HttpPostedFileBase uploadFile)
        {
            try
            {
                //Kiểm tra tính hợp lệ của dữ liệu được nhập
                if (string.IsNullOrEmpty(model.LastName))
                {
                    ModelState.AddModelError("LastName", "Last Name is invalid");
                }
                if (string.IsNullOrEmpty(model.FirstName))
                {
                    ModelState.AddModelError("FirstName", "First Name is invalid");
                }
                if (string.IsNullOrEmpty(model.Title))
                {
                    model.Title = "";
                }
                if (model.BirthDate == null)
                {
                    model.BirthDate = Convert.ToDateTime("1800/01/01");
                }
                if (model.HireDate == null)
                {
                    model.HireDate = Convert.ToDateTime("1818/01/01");
                }
                else
                {
                    if (model.HireDate.Year - model.BirthDate.Year < 18)
                    {
                        ModelState.AddModelError("HireDate", "Employee must be at least 18 years old");
                    }
                }
                if (string.IsNullOrEmpty(model.Email))
                {
                    model.Email = "";
                }
                if (string.IsNullOrEmpty(model.Address))
                {
                    model.Address = "";
                }
                if (string.IsNullOrEmpty(model.City))
                {
                    model.City = "";
                }
                if (string.IsNullOrEmpty(model.Country))
                {
                    model.Country = "";
                }
                if (string.IsNullOrEmpty(model.HomePhone))
                {
                    model.HomePhone = "";
                }
                if (string.IsNullOrEmpty(model.Notes))
                {
                    model.Notes = "";
                }
                if (string.IsNullOrEmpty(model.PhotoPath))
                {
                    Employee existEmployee = EmployeeBLL.GetEmployee(model.EmployeeID);
                    model.PhotoPath = existEmployee != null ? existEmployee.PhotoPath : "";
                }
                if (string.IsNullOrEmpty(model.Password))
                {
                    model.Password = MD5.EncodeMD5("123456");
                }

                //Upload ảnh  
                if (uploadFile != null)
                {
                    string _FileName = Path.GetFileName(uploadFile.FileName);
                    _FileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(_FileName);
                    string _path = Path.Combine(Server.MapPath("~/Uploads/Images"), _FileName);
                    uploadFile.SaveAs(_path);
                    model.PhotoPath = _FileName;
                }

                if (ModelState.IsValid)
                {
                    //Lưu dữ liệu vào DB
                    if (model.EmployeeID == 0)
                    {
                        EmployeeBLL.AddEmployee(model);
                    }
                    else
                    {
                        EmployeeBLL.UpdateEmployee(model);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ": " + ex.StackTrace);
            }
            return View(model);
        }
        /// <summary>
        /// xoa
        /// </summary>
        /// <param name="employeeIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] employeeIDs)
        {
            if (employeeIDs != null)
            {
                EmployeeBLL.DeleteEmployee(employeeIDs);
            }
            return RedirectToAction("Index");

        }
    }
}