using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Web.Core.Dto;
using Web.Core.Util;
using Web.Watch.Models;
using Web.Watch.Models.Payments;
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
        public ActionResult About()
        {
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
        public long GenerateSecretCode()
        {
            DateTime currentDateTime = DateTime.Now;

            // Format the date and time to the desired format: YYMMDDHHmmss
            string formattedDateTime = currentDateTime.ToString("yyMMddHHmmss");

            // Convert the formatted string to a long
            if (long.TryParse(formattedDateTime, out long secretCode))
            {
                return secretCode;
            }
            else
            {
                // Handle parsing failure if necessary
                throw new InvalidOperationException("Failed to generate secret code.");
            }
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
            MvcApplication.SC = GenerateSecretCode();
            order.SecretCode = MvcApplication.SC;
            var orderDTO = this.orderService.Insert(order);
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
                    PaymentWithVnpay(orderDTO, MvcApplication.SC.ToString());
                }
                else
                {
                    if (order.PaymentMethod == "COD")
                    {
                        var paymentinfo = new PaymentInfomation()
                        {
                            TransactionStatus = "00",
                            ResponseCode = "00"
                        };
                        return View("OrderSuccess", paymentinfo);
                    }
                }
            }
            return View();
        }

        public string getStringAppSettingVNPayTest()
        {
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
            return vnp_Returnurl + " " + vnp_Url + " " + vnp_TmnCode + " " + vnp_HashSecret;
        }

        protected void PaymentWithVnpay(OrderDto order, string id)
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
            vnpay.AddRequestData("vnp_TxnRef", id);
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
        public ActionResult OrderSuccess(string vnp_Amount, string vnp_BankCode, string vnp_BankTranNo, string vnp_CardType, string vnp_OrderInfo, string vnp_PayDate, string vnp_ResponseCode, string vnp_TransactionNo, string vnp_TransactionStatus)
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
        [HttpPost]
        public ActionResult QueryUserByPhonenumber(string phonenumber)
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
        public ActionResult PaymentWithPaypal(OrderDto order, string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            PaymentInfomation paymentinfo = null;
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
                        paymentinfo = new PaymentInfomation()
                        {
                            TransactionStatus = "01",
                            ResponseCode = "01"
                        };
                        //orderService.DeleteById(orderService.GetBySecretCode(MvcApplication.SC).Id);
                        return View("OrderSuccess", paymentinfo);
                    }
                }
            }
            catch (Exception ex)
            {
                paymentinfo = new PaymentInfomation()
                {
                    TransactionStatus = "01",
                    ResponseCode = "01"
                };
                //orderService.DeleteById(orderService.GetBySecretCode(MvcApplication.SC).Id);
                return View("OrderSuccess", paymentinfo);
            }
            //on successful payment, show success page to user.  
            paymentinfo = new PaymentInfomation()
            {
                TransactionStatus = "00",
                ResponseCode = "00"
            };
            return View("OrderSuccess", paymentinfo);
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


        [HttpPost]
        public ActionResult GetAllMenu()
        {
            var menu = menuService.GetAllActive();
            return Json(menu, JsonRequestBehavior.AllowGet);
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