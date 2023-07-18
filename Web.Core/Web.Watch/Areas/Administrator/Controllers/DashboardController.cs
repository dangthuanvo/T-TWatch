using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Watch.Service;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class DashboardController : BaseController
    {
        ReportService reportService;

        public DashboardController()
        {
            this.reportService = new ReportService();
        }
        // GET: Administrator/Dashboard
        public ActionResult Index()
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            ViewData["data"] = this.reportService.GetHighlight();
            return View();
        }
    }
}