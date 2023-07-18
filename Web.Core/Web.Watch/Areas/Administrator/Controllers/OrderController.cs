using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Core.Dto;
using Web.Watch.Service;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class OrderController : BaseController
    {
        public OrderService orderService { get; set; }
        public OrderController()
        {
            this.orderService = new OrderService();
        }

        // GET: Administrator/Order
        public ActionResult Index()
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            return View(this.orderService.GetAll());
        }

        public ActionResult Update(int id)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            return View(this.orderService.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(OrderDto order)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            this.orderService.Update(order.Id, order);
            return RedirectToAction("Index");
        }

    }
}