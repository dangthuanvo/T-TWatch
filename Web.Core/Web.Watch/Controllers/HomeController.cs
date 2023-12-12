using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Core.Dto;
using Web.Watch.Service;

namespace Web.Watch.Controllers
{
    public class HomeController : Controller
    {
        WebsiteService websiteService;
        ProductService productService;
        MenuService menuService;
        GalleryService galleryService;
        OrderService orderService;
        ArticleService articleService;

        public HomeController()
        {
            this.websiteService = new WebsiteService();
            this.productService = new ProductService();
            this.menuService = new MenuService();
            this.galleryService = new GalleryService();
            this.orderService = new OrderService();
            this.articleService = new ArticleService();
        }

        public ActionResult Index()
        {
            this.SetSEO_Main();
            ViewData["galleries"] = this.galleryService.GetAll();
            ViewData["sellings"] = this.productService.GetAllSelling();
            ViewData["menus"] = this.menuService.GetAllShowHomePage();

            return View();
        }

        public ActionResult Category(string alias, string orderBy = "")
        {
            ViewBag.orderBy = orderBy;
            MenuDto menu = new MenuDto();
            List<ProductDto> products = null;
            if (alias == "all-1")
            {
                menu.Alias = "all-1";
                products = productService.GetAllOrder(orderBy);
            }
            else
            {
                menu = menuService.GetByAlias(alias);
                products = productService.GetByMenu(menu.Id, orderBy);
            }
            menu.Products = products;
            ViewBag.MetaTitle = menu.Name;
            ViewBag.MetaDescription = menu.MetaDescription;
            ViewBag.MetaRobots = menu.MetaRobots;
            ViewBag.MetaRevisitAfter = menu.MetaRevisitAfter;
            ViewBag.MetaContentLanguage = menu.MetaContentLanguage;
            ViewBag.MetaContentType = menu.MetaContentType;
            ViewData["menuall"] = this.menuService.GetAll();
            return View(menu);
        }

        public ActionResult ProductDetail(string alias)
        {
            ProductDto product = this.productService.GetByAlias(alias);
            List<ProductDto> products = this.productService.GetByMenu(product.MenuId.Value);
            ViewData["products"] = products;

            ViewBag.MetaTitle = product.Name;
            ViewBag.MetaDescription = product.MetaDescription;
            ViewBag.MetaRobots = product.MetaRobots;
            ViewBag.MetaRevisitAfter = product.MetaRevisitAfter;
            ViewBag.MetaContentLanguage = product.MetaContentLanguage;
            ViewBag.MetaContentType = product.MetaContentType;
            return View(product);
        }

        public ActionResult Buy(int id)
        {
            List<OrderDetailDto> cart = (List<OrderDetailDto>)Session["cart"];
            if (cart == null)
            {
                cart = new List<OrderDetailDto>();
            }

            if (cart.Any(x => x.ProductId == id))
            {
                cart.FirstOrDefault(x => x.ProductId == id).Qty++;
            }
            else
            {
                ProductDto product = this.productService.GetById(id);
                cart.Add(new OrderDetailDto()
                {
                    ProductId = product.Id,
                    ProductImage = product.Image,
                    Qty = 1,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    ProductDiscountPrice = product.DiscountPrice
                });
            }
            Session["cart"] = cart;
            Session["cartCount"] = cart.Sum(x => x.Qty ?? 0);
            return RedirectToAction("ShoppingCart");
        }


        [HttpPost]
        public ActionResult UpdateCart(List<OrderDetailDto> products)
        {
            List<OrderDetailDto> cart = (List<OrderDetailDto>)Session["cart"];
            if (cart == null)
            {
                cart = new List<OrderDetailDto>();
            }

            cart.ForEach(x =>
            {
                x.Qty = products.FirstOrDefault(y => y.ProductId == x.ProductId).Qty;
            });

            Session["cart"] = cart.FindAll(x => x.Qty > 0);
            Session["cartCount"] = cart.Sum(x => x.Qty ?? 0);
            return RedirectToAction("ShoppingCart");
        }

        public ActionResult ShoppingCart()
        {
            this.SetSEO_Main();
            List<OrderDetailDto> cart = (List<OrderDetailDto>)Session["cart"];
            if (cart == null)
            {
                cart = new List<OrderDetailDto>();
            }
            return View(cart);
        }

        [HttpPost]
        public ActionResult Order(OrderDto order)
        {
            List<OrderDetailDto> cart = (List<OrderDetailDto>)Session["cart"];
            if (cart == null || cart.Count == 0)
            {
                return RedirectToAction("ShoppingCart");
            }

            order.OrderDetails = cart;
            this.orderService.Insert(order);
            Session["cart"] = null;
            Session["cartCount"] = null;
            return RedirectToAction("OrderSuccess");
        }

        public ActionResult OrderSuccess()
        {
            this.SetSEO_Main();
            return View();
        }

        public ActionResult Article(string alias)
        {
            this.SetSEO_Main();
            ViewData["articles"] = this.articleService.GetAll();
            return View(this.articleService.GetByAlias(alias));
        }

        public ActionResult Search(string q = "", string orderBy = "")
        {
            this.SetSEO_Main();
            ViewBag.q = q;
            ViewBag.orderBy = orderBy;
            List<ProductDto> products = this.productService.Search(q, orderBy);
            return View(products);
        }


        public void SetSEO_Main()
        {
            WebsiteDto website = this.websiteService.GetAll().FirstOrDefault();
            ViewBag.MetaTitle = website.MetaTitle;
            ViewBag.MetaDescription = website.MetaDescription;
            ViewBag.MetaRobots = website.MetaRobots;
            ViewBag.MetaRevisitAfter = website.MetaRevisitAfter;
            ViewBag.MetaContentLanguage = website.MetaContentLanguage;
            ViewBag.MetaContentType = website.MetaContentType;
        }
    }
}