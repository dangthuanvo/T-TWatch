using System.Web.Mvc;
using Web.Watch.Service;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class ImportHistoryController : BaseController
    {
        public ImportHistoryService importHistoryService;

        public ImportHistoryController()
        {
            this.importHistoryService = new ImportHistoryService();
        }
        // GET: Administrator/Article 
        public ActionResult Index()
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            return View(this.importHistoryService.GetAll());
        }
    }
}