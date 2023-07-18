using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Watch.Service;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class ReportController : BaseController
    {
        ReportService reportService;

        public ReportController()
        {
            this.reportService = new ReportService();
        }

        // GET: Administrator/Report
        public ActionResult Index(DateTime? startDate = null, DateTime? toDate = null)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            if (!startDate.HasValue)
                startDate = DateTime.Now.AddDays(-30).Date;

            if (!toDate.HasValue)
                toDate = DateTime.Now.Date;

            ViewBag.startDate = startDate;
            ViewBag.toDate = toDate;

            return View(this.reportService.GetGeneralReport(startDate, toDate));
        }

        public ActionResult GetRevenueReport(DateTime? date)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            if (!date.HasValue)
                date = DateTime.Now.Date;

            ViewBag.date = date;

            return View(this.reportService.GetRevenueReport(date.Value));
        }
    }
}