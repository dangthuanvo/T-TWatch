using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Core.Dto;
using Web.Watch.Service;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class WebsiteController : BaseController
    {
        WebsiteService websiteService;

        public WebsiteController()
        {
            this.websiteService = new WebsiteService();
        }

        public ActionResult Index()
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            return View(this.websiteService.GetAll().FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(WebsiteDto website)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            this.websiteService.Update(website.Id, website);
            return RedirectToAction("Index");
        }
    }
}