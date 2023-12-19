﻿using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Core.Dto;
using Web.Core.Util;
using Web.Watch.Service;
using static Web.Watch.Models.PaypalConfiguaration;

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
        public ActionResult Comment(string email, int productid)
        {
            this.SetSEO_Main();
            bool ok = orderService.VerifyAbilityToComment(email, productid);
            return Json(ok);
        }
        public ActionResult SubmitComment(string productid, string email, string comment, string star)
        {
            var product = productService.GetById(int.Parse(productid));
            var customer = customerService.GetByEmail(email);
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

        public ActionResult TrackingSendEmail(string email)
        {
            this.SetSEO_Main();
            Random random = new Random();

            // Sinh số ngẫu nhiên từ 100,000 đến 999,999
            MvcApplication.OTP = random.Next(100000, 1000000).ToString();
            orderService.SendOTP(email, MvcApplication.OTP);
            return Json(true);
        }
        public ActionResult TrackingConfirmedEmail(string EnterOTP)
        {
            this.SetSEO_Main();
            // Sinh số ngẫu nhiên từ 100,000 đến 999,999
            if (EnterOTP == MvcApplication.OTP)
                return Json(true);
            return Json(false);
        }
        public ActionResult Tracking(string email)
        {
            this.SetSEO_Main();
            var orders = orderService.GetByEmail(email);
            ViewBag.emailTracking = email;
            orders.Reverse();
            Random random = new Random();
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
            this.orderService.Insert(order);
            foreach (var item in cart)
            {
                var product = productService.GetById(item.ProductId);
                product.Quantity -= (int)item.Qty;
                productService.Update(product.Id, product);
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
            if (order.PaymentMethod == "Paypal")
            {
                return RedirectToAction("PaymentWithPaypal", order);
            }
            else
            {
                if (order.PaymentMethod == "VnPay")
                {
                    return RedirectToAction("PaymentWithVnpay");
                }
                else
                {
                    if (order.PaymentMethod == "COD")
                    {
                        return RedirectToAction("OrderSuccess");
                    }
                }
            }
            return View();
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
        [HttpPost]
        public ActionResult QueryUserByEmail(string email)
        {

            List<CustomerDto> customers = this.customerService.GetAll();
            var customer = customers.FirstOrDefault(c => string.Equals(c.Email, email, StringComparison.OrdinalIgnoreCase));

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
        public ActionResult PaymentWithPaypal(OrderDto order, string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(order, apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("OrderFailure");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("OrderFailure");
            }
            //on successful payment, show success page to user.  
            return View("OrderSuccess");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(OrderDto order, APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            foreach (var item in order.OrderDetails)
            {
                itemList.items.Add(new Item()
                {
                    name = item.ProductName,
                    currency = "USD",
                    price = (item.ProductDiscountPrice / 24368).ToString(),
                    quantity = item.Qty.ToString(),
                    sku = "sku"
                });
            }

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = Math.Round((double)order.TotalAmount / 24000, 2).ToString(),
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = Math.Round((double)order.TotalAmount / 24000, 2).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
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