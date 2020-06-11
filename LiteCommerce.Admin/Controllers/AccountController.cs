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
            return RedirectToAction("Index", "Product");
            //TODO: Kiem tra tai khoan thong qua co so du lieu
            //password = MD5.EncodeMD5(password);
            //UserAccount user = UserAccountBLL.Authorize(email, password, UserAccountTypes.Employee);
            //if (user != null) //Đăng nhập thành công
            //{
            //    //Ghi nhận phiên đăng nhập
            //    WebUserData userData = new WebUserData()
            //    {
            //        UserID = user.UserID,
            //        FullName = user.FullName,
            //        GroupName = Convert.ToString(UserAccountTypes.Employee), //TODO: cần thay đổi cho đúng
            //        LoginTime = DateTime.Now,
            //        SessionID = Session.SessionID,
            //        ClientIP = Request.UserHostAddress,
            //        Photo = user.Photo,
            //        Title = user.Title
            //    };
            //    FormsAuthentication.SetAuthCookie(userData.ToCookieString(), false);
            //    return RedirectToAction("Index", "Dashboard");
            //}
            //else // Đăng nhập không thành công
            //{
            //    ModelState.AddModelError("loginerror", "login fail");
            //    ViewBag.email = email;
            //    return View();
            //}

            //if (email == "qwerty@gmail.com" && password == "12345678")
            //{
            //    //Ghi nhan phien dang nhap tai khoan
            //    FormsAuthentication.SetAuthCookie(email, false);
            //    // Chuyen trang ve Dashboard 
            //    return RedirectToAction("Index", "Dashboard");
            //}
            //else
            //{
            //    ModelState.AddModelError("LoginError", "Login Fail");
            //    ViewBag.Email = email;
            //    return View();
            //}
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

    }
}