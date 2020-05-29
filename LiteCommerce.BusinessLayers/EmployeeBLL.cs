using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class EmployeeBLL
    {
        private static IEmployeeDAL EmployeeDB;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            EmployeeDB = new DataLayers.SqlServer.EmployeeDAL(connectionString);
        }

        /// <summary>
        /// Lấy danh sách các Employees. Có phân trang.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
            {
                pageSize = 20;
            }
            rowCount = EmployeeDB.Count(searchValue);
            return EmployeeDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static Employee GetEmployee(int employeeID)
        {
            return EmployeeDB.Get(employeeID);
        }

        public static bool UpdateEmployee(Employee employee)
        {
            return EmployeeDB.Update(employee);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static int AddEmployee(Employee employee)
        {
            return EmployeeDB.Add(employee);
        }

        public static int DeleteEmployee(int[] employeeIDs)
        {
            return EmployeeDB.Delete(employeeIDs);
        }
    }
}
