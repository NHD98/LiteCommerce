using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            //TODO: Kiem tra tai khoan thong qua co so du lieu
            if (email == "qwerty@gmail.com" && password == "12345678")
            {
                //Ghi nhan phien dang nhap tai khoan
                System.Web.Security.FormsAuthentication.SetAuthCookie(email, false);
                // Chuyen trang ve Dashboard 
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("LoginError", "Login Fail");
                ViewBag.Email = email;
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