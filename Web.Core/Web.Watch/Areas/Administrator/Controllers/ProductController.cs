using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Core.Dto;
using Web.Watch.Service;

namespace Web.Watch.Areas.Administrator.Controllers
{
    public class ProductController : BaseController
    {
        ProductService productService;
        MenuService menuService;
        AttributeService attributeService;
        ImportHistoryService importHistoryService;
        ReviewService reviewService;

        public ProductController()
        {
            this.reviewService = new ReviewService();
            this.importHistoryService = new ImportHistoryService();
            this.productService = new ProductService();
            this.menuService = new MenuService();
            this.attributeService = new AttributeService();
        }

        // GET: Administrator/Product
        public ActionResult Index()
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            return View(this.productService.GetAll());
        }

        public ActionResult Add()
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            ViewData["menus"] = this.menuService.GetAll();
            ViewData["attributes"] = this.attributeService.GetAll().Select(x => new ProductAttributeDto()
            {
                Attribute = x,
                AttributeId = x.Id,
                ValueString = ""
            }).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductDto product)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();
            product.Quantity = 0;
            this.productService.Insert(product);
            return RedirectToAction("Index");
        }

        public ActionResult Update(int id)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            ViewData["menus"] = this.menuService.GetAll();
            List<ProductAttributeDto> attributes = this.attributeService.GetAll().Select(x => new ProductAttributeDto()
            {
                Attribute = x,
                AttributeId = x.Id,
                ValueString = ""
            }).ToList();

            ProductDto product = this.productService.GetById(id);
            var temp = product.Quantity;

            if (product.ProductAttributes != null)
            {
                attributes.ForEach(x =>
                {
                    var attrExist = product.ProductAttributes.FirstOrDefault(y => y.AttributeId == x.AttributeId);
                    if (attrExist != null)
                    {
                        x.ValueString = attrExist.ValueString;
                    }
                });
                product.Quantity = temp;
            }

            ViewData["attributes"] = attributes;
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ProductDto product)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();
            product.Quantity = productService.GetById(product.Id).Quantity;
            this.productService.Update(product.Id, product);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult UpdateComment(List<ReviewActiveDto> reviewactive)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();
            foreach (var review in reviewactive)
            {
                var reviewDto = reviewService.GetById((int)review.Id);
                reviewDto.Active = (int)review.Active;
                reviewService.Update(reviewDto.Id, reviewDto);
            }
            return Json(true);
        }

        public ActionResult Delete(int id)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            this.productService.DeleteById(id);
            return RedirectToAction("Index");
        }

        public ActionResult ImportIndex(int id)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();
            ProductDto product = this.productService.GetById(id);
            ViewData["currentAdmin"] = MvcApplication.currentAdmin;
            ViewData["product"] = product;
            ImportProductDto importProduct = new ImportProductDto()
            {
                Product = product,
            };
            return View(importProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(ImportProductDto importProduct)
        {
            if (!this.CheckAuth())
                return this.RedirectToLogin();

            this.importHistoryService.Insert(importProduct.ImportHistory);
            ProductDto product = productService.GetById(importProduct.ImportHistory.ProductID);
            product.Quantity += importProduct.ImportHistory.Quantity;
            productService.Update(product.Id, product);
            return RedirectToAction("Index");
        }
    }
}