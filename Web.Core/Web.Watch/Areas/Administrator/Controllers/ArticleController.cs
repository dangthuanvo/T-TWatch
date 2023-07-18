using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Core.Dto;
using Web.Watch.Service;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class ArticleController : BaseController
    {
        public ArticleService articleService;

        public ArticleController()
        {
            this.articleService = new ArticleService();
        }
        // GET: Administrator/Article 
        public ActionResult Index()
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            return View(this.articleService.GetAll());
        }

        public ActionResult Update(int id)
        {
            if (!this.CheckAuth())
                this.RedirectToLogin();

            return View(this.articleService.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ArticleDto article)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            this.articleService.Update(article.Id, article);
            return RedirectToAction("Index");
        }
    }
}