using System;
using System.Web.Mvc;
using Web.Core.Dto;
using Web.Watch.Service;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class LoginController : Controller
    {
        UserService userService;

        public LoginController()
        {
            this.userService = new UserService();
        }
        // GET: Administrator/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserDto user)
        {
            try
            {
                UserDto u = this.userService.CheckLogin(user);
                Session["user_admin"] = u;
                MvcApplication.currentAdmin = u.UserName;
            }
            catch (Exception ex)
            {
                Session["error"] = ex.Message;
                return View();
            }

            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult LogOut()
        {
            Session["user_admin"] = null;

            return RedirectToAction("Index");
        }
    }
}