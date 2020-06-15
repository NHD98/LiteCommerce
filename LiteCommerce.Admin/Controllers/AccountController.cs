using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LiteCommerce.Admin.Controllers
{/// <summary>
/// 
/// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(string userID, string oldPassword, string newPassword, string confirmPassword)
        {
            //kiểm tra hợp lệ dữ liệu
            if (string.IsNullOrEmpty(userID))
            {
                ModelState.AddModelError("userID", "UserID is invalid");
            }
            if (string.IsNullOrEmpty(oldPassword))
            {
                ModelState.AddModelError("oldPassword", "Old Password is invalid");
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError("newPassword", "New Password is invalid");
            }
            if (string.IsNullOrEmpty(confirmPassword))
            {
                ModelState.AddModelError("confirmPassword", "Confirm Password is invalid");
            }
            if (!newPassword.Equals(confirmPassword))
            {
                ModelState.AddModelError("notMatch", "New Password and Confirm Password must match");
            }
            Employee existEmployee = EmployeeBLL.GetEmployee(Convert.ToInt32(userID));
            oldPassword = MD5.EncodeMD5(oldPassword);
            newPassword = MD5.EncodeMD5(newPassword);
            if (!existEmployee.Password.Equals(oldPassword))
            {
                ModelState.AddModelError("wrongPassword", "Password is wrong");
            }

            if (ModelState.IsValid)
            {
                //Lưu thay đổi
                if (EmployeeBLL.ChangePassword(Convert.ToInt32(userID), newPassword))
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SignOut()
        {
            Session.Abandon();
            Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");
            return View();

        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(string email = "", string password = "")
        {
            //TODO: Kiem tra tai khoan thong qua co so du lieu
            password = MD5.EncodeMD5(password);
            UserAccount user = UserAccountBLL.Authorize(email, password, UserAccountTypes.Employee);
            if (user != null) //Đăng nhập thành công
            {
                //Ghi nhận phiên đăng nhập
                WebUserData userData = new WebUserData()
                {
                    UserID = user.UserID,
                    FullName = user.FullName,
                    GroupName = user.Roles, //TODO: cần thay đổi cho đúng
                    LoginTime = DateTime.Now,
                    SessionID = Session.SessionID,
                    ClientIP = Request.UserHostAddress,
                    Photo = user.Photo,
                    Title = user.Title
                };
                FormsAuthentication.SetAuthCookie(userData.ToCookieString(), false);
                return RedirectToAction("Index", "Dashboard");
            }
            else // Đăng nhập không thành công
            {
                ModelState.AddModelError("loginerror", "login fail");
                ViewBag.email = email;
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

    }
}