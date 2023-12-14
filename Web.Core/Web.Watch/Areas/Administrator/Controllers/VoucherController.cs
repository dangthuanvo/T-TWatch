using System.Web.Mvc;
using Web.Core.Dto;
using Web.Watch.Service;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class VoucherController : BaseController
    {
        public VoucherService voucherService;

        public VoucherController()
        {
            this.voucherService = new VoucherService();
        }
        // GET: Administrator/Voucher 
        public ActionResult Index()
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            return View(this.voucherService.GetAll());
        }

        public ActionResult Update(int id)
        {
            if (!this.CheckAuth())
                this.RedirectToLogin();

            return View(this.voucherService.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(VoucherDto voucher)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            this.voucherService.Update(voucher.Id, voucher);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            this.voucherService.DeleteById(id);
            return RedirectToAction("Index");


        }
        public ActionResult Add()
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(VoucherDto voucher)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();
            this.voucherService.Insert(voucher);
            return RedirectToAction("Index");
        }
    }
}