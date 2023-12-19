using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Web.Mvc;
using Web.Core.Dto;
using Web.Core.Util;
using Web.Watch.Models;
using Web.Watch.Models.Payments;
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
        CustomerService customerService;
        ReviewService reviewService;
        VoucherService voucherService;
        public HomeController()
        {
            this.websiteService = new WebsiteService();
            this.productService = new ProductService();
            this.menuService = new MenuService();
            this.galleryService = new GalleryService();
            this.orderService = new OrderService();
            this.articleService = new ArticleService();
            this.customerService = new CustomerService();
            this.reviewService = new ReviewService();
            this.voucherService = new VoucherService();
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
            List<ReviewDto> reviews = reviewService.GetByProductID(product.Id);
            List<ProductDto> products = this.productService.GetByMenu(product.MenuId.Value);
            ViewData["products"] = products;

            ViewBag.MetaTitle = product.Name;
            ViewBag.MetaDescription = product.MetaDescription;
            ViewBag.MetaRobots = product.MetaRobots;
            ViewBag.MetaRevisitAfter = product.MetaRevisitAfter;
            ViewBag.MetaContentLanguage = product.MetaContentLanguage;
            ViewBag.MetaContentType = product.MetaContentType;
            ProductReviewDto productreviews = new ProductReviewDto()
            {
                Product = product,
                Reviews = reviews
            };
            return View(productreviews);
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
            OrderDetailVoucherDto cartdetail = new OrderDetailVoucherDto();
            List<OrderDetailDto> cart = (List<OrderDetailDto>)Session["cart"];
            if (cart == null)
            {
                cart = new List<OrderDetailDto>();
            }
            cartdetail.OrderDetails = cart;
            cartdetail.Vouchers = voucherService.GetAllAvailable();
            return View(cartdetail);
        }
        public ActionResult Comment(string phonenumber, int productid)
        {
            this.SetSEO_Main();
            bool ok = orderService.VerifyAbilityToComment(phonenumber, productid);
            return Json(ok);
        }
        public ActionResult SubmitComment(string productid, string phonenumber, string comment, string star)
        {
            var product = productService.GetById(int.Parse(productid));
            var customer = customerService.GetByPhoneNumber(phonenumber);
            product.RateAmount += 1;
            product.Rate = Math.Round((product.Rate * (product.RateAmount - 1.0) + double.Parse(star)) / product.RateAmount, 1);
            ReviewDto review = new ReviewDto()
            {
                ProductId = int.Parse(productid),
                Star = int.Parse(star),
                Content = comment,
                Active = 1,
                CustomerName = customer.FullName,
                Created = DateTime.Now,
                CustomerCode = customer.Code,
            };
            reviewService.Insert(review);
            productService.Update(product.Id, product);
            return Json(true);
        }
        public ActionResult Tracking(string phonenumber)
        {
            this.SetSEO_Main();
            var orders = orderService.GetByPhoneNumber(phonenumber);
            ViewBag.phoneTracking = phonenumber;

            return View(orders);
        }
        public ActionResult ViewTracking(int id)
        {
            return View(this.orderService.GetById(id));
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
            order.TotalAmount = TotalAfterDiscount(order.VoucherId.ToString(), order.TotalAmount.ToString());
            order.OrderDate = DateTime.Now;
            //Thực hiện thanh toán
            //3 phương thức:
            //order.PaymentMethod == 1/2/3
            //1: COD
            //2: VNPay
            //3: Paypal
            string payId = Guid.NewGuid().ToString();
            var orderDTO = this.orderService.Insert(order);
            foreach (var item in cart)
            {
                var product = productService.GetById(item.ProductId);
                product.Quantity -= (int)item.Qty;
                productService.Update(product.Id, product);
            }
            if (order.PaymentMethod == "1")
            {
                //next
            }
            else if (order.PaymentMethod == "2")
            {
                VNPayProcess(order, payId);
            }
            else if (order.PaymentMethod == "3")
            {
                return RedirectToAction("OrderFailed");
            }

            try
            {
                VoucherDto voucher = voucherService.GetById((int)order.VoucherId);
                voucher.IsActive -= 1;
                voucherService.Update(voucher.Id, voucher);
            }
            catch (Exception ex) { }
            Session["cart"] = null;
            Session["cartCount"] = null;


            return RedirectToAction("OrderSuccess");
        }

        public string getStringAppSettingVNPayTest()
        {
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
            return vnp_Returnurl + " " + vnp_Url + " " + vnp_TmnCode + " " + vnp_HashSecret;
        }

        protected void VNPayProcess(OrderDto order, string id)
        {
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Get payment input
            //OrderInfo order = new OrderInfo();
            //order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            //order.Amount = 100000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            //order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
            //order.CreatedDate = DateTime.Now;
            //Save order to db

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.TotalAmount * 100).ToString()); 
            //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ.
            //Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", order.OrderDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng:" + order.Id);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", id.ToString()); 
            // Mã tham chiếu của giao dịch tại hệ thống của merchant.
            //  Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY.
            // Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //return true;
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            Response.Redirect(paymentUrl);
        }

        [HttpPost]
        public ActionResult GetUpdatedTotalAmount(string voucherId, string total)
        {

            var doubleTotal = double.Parse(total.Replace(".", ""));
            double res = 0;

            var voucher = voucherService.GetById(int.Parse(voucherId));
            if (voucher.Type == 0)
            {
                res = doubleTotal - (double)voucher.DiscountAmount;
            }
            else
            {
                var amount = (1 - (double)voucher.DiscountAmount / 100);
                res = (int)Math.Round(amount * doubleTotal);
            }
            return Json(DataHelper.ToCurrency(res));
        }
        public double TotalAfterDiscount(string voucherId, string total)
        {
            VoucherDto voucher = new VoucherDto();

            var doubleTotal = double.Parse(total.Replace(".", ""));
            double res = 0;
            try
            {
                voucher = voucherService.GetById(int.Parse(voucherId));
            }
            catch (Exception ex) { return double.Parse(total); };
            if (voucher.Type == 0)
            {
                res = doubleTotal - (double)voucher.DiscountAmount;
            }
            else
            {
                var amount = (1 - (double)voucher.DiscountAmount / 100);
                res = (int)Math.Round(amount * doubleTotal);
            }
            return res;
        }
        public ActionResult OrderSuccess(string vnp_Amount,string vnp_BankCode, string vnp_BankTranNo,string vnp_CardType, string vnp_OrderInfo,string vnp_PayDate, string vnp_ResponseCode, string vnp_TransactionNo, string vnp_TransactionStatus)
        {
            PaymentInfomation info = new PaymentInfomation(vnp_Amount, vnp_BankCode, vnp_BankTranNo, vnp_CardType, vnp_OrderInfo, vnp_PayDate, vnp_ResponseCode, vnp_TransactionNo, vnp_TransactionStatus);
            this.SetSEO_Main();
            return View(info);
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
        [HttpPost]
        public ActionResult QueryUserByPhoneNumber(string phonenumber)
        {

            List<CustomerDto> customers = this.customerService.GetAll();
            var customer = customers.FirstOrDefault(c => string.Equals(c.PhoneNumber, phonenumber, StringComparison.OrdinalIgnoreCase));

            if (customer != null)
            {
                return Json(customer);
            }
            else
            {
                // Return a 404 status code if the customer is not found
                return null;
            }
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