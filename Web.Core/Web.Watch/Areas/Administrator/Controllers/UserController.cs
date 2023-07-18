using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Core.Dto;
using Web.Watch.Service;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class UserController : BaseController
    {
        UserService userService;
        public UserController()
        {
            this.userService = new UserService();
        }

        // GET: Administrator/User
        public ActionResult Index()
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            return View(this.userService.GetAll());
        }

        public ActionResult Add()
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            return View(new UserDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(UserDto user)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            try
            {
                this.userService.Insert(user);
            }
            catch (Exception ex)
            {
                Session["error"] = ex.Message;
                return View(user);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Update(string id)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            return View(this.userService.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UserDto user)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            this.userService.Update(user.UserName, user);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            this.userService.DeleteById(id);
            return RedirectToAction("Index");
        }

        public ActionResult ResetPassword(string id, string newPassword)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            try
            {
                this.userService.ResetPassword(id, newPassword);
            }
            catch (Exception ex)
            {
                Session["error"] = ex.Message;
                return RedirectToAction("Update", new
                {
                    id
                });
            }
            return RedirectToAction("Index");
        }
    }
}