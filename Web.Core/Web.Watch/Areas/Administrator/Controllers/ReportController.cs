using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.Core.Dto;
using Web.Watch.Service;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class ReportController : BaseController
    {
        ReportService reportService;
        OrderService orderService;
        public ReportController()
        {
            this.orderService = new OrderService();
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
        public ActionResult ExportExcel(List<int> orderIds, string sDate, string eDate)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();
            List<OrderDto> orders = new List<OrderDto>();
            foreach (var item in orderIds)
            {
                orders.Add(orderService.GetById(item));
            }
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Report");
            ws.Cell("A1").Value = "Báo cáo tổng hợp của hàng bán đồng hồ T&T Watch" + " từ ngày " + sDate + " tới ngày " + eDate;
            ws.Cell("A2").Value = "Id";
            ws.Cell("B2").Value = "Ngày đặt hàng";
            ws.Cell("C2").Value = "Tên khách hàng";
            ws.Cell("D2").Value = "Số điện thoại";
            ws.Cell("E2").Value = "Email";
            ws.Cell("F2").Value = "Địa chỉ";
            ws.Cell("G2").Value = "Tổng tiền";
            ws.Cell("H2").Value = "Trạng thái";
            int row = 3;
            double? total = 0;
            foreach (var item in orders)
            {
                ws.Cell("A" + row).Value = item.Id;
                ws.Cell("B" + row).Value = item.OrderDate;
                ws.Cell("C" + row).Value = item.Customer.FullName;
                ws.Cell("D" + row).Value = item.Customer.PhoneNumber;
                ws.Cell("E" + row).Value = item.Customer.Email;
                ws.Cell("F" + row).Value = item.Customer.Address;
                ws.Cell("G" + row).Value = item.TotalAmount;
                total += item.TotalAmount;
                ws.Cell("H" + row).Value = "Đã hoàn thành";
                row++;
            }
            ws.Cell("F" + row).Value = "Tổng cộng";
            ws.Cell("G" + row).Value = total;
            string fileName = "\\Report_" + DateTime.Now.Ticks + ".xlsx";
            string pathFile = AppContext.BaseDirectory + "\\Resources\\Report" + fileName;
            wb.SaveAs(pathFile);

            return Json(fileName);
        }
    }
}