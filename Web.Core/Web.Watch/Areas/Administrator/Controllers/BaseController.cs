using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class BaseController : Controller
    {
        public bool CheckAuth()
        {
            return Session["user_admin"] != null;
        }

        public ActionResult RedirectToLogin()
        {
            return RedirectToAction("Index", "Login");
        }
    }
}