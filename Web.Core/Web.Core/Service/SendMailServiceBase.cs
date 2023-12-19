
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Web.Core.Model;

namespace Web.Core.Service
{
    public class SendEmailServiceBase
    {
        public void SendEmail(Customer customer, List<OrderDetail> orderdetail, Order order)
        {
            try
            {
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "dangthuanvo1611@gmail.com";
                string smtpPassword = "pwuebwfhtmnujdwt";
                string body = System.IO.File.ReadAllText(AppContext.BaseDirectory.Replace(@"\Web.Watch", "") + "\\Web.Core\\" + "Service\\EmailTemplate\\SendMailTemplate.html");
                string listProduct = null;
                string first_name = customer.FullName;
                string order_id = order.Id.ToString();
                string status = string.Empty;
                string voucher_code = string.Empty;
                using (var context = new MyContext())
                {
                    try
                    {
                        voucher_code = context.Vouchers.Where(x => x.Id == order.VoucherId).FirstOrDefault().VoucherCode;
                    }
                    catch { }

                }
                if (order.Status == 10 || order.Status == 20)
                {
                    status = "Your order has been received";
                }
                else
                {
                    if (order.Status == 30)
                    {
                        status = "Your order has been cancelled";
                    }
                    else
                    {
                        if (order.Status == 40)
                        {
                            status = "Your order has been delivered to the shipping unit";
                        }
                        else
                        {
                            if (order.Status == 50)
                            {
                                status = "Your order has been completed";
                            }
                        }
                    }
                }

                string order_shipping_address = customer.Address.ToString();
                string order_total_price = order.TotalAmount.ToString();
                string company_name = "T&TWatch";
                string company_address = "dangthuanvo1611@gmail.com";
                foreach (var productData in orderdetail)
                {
                    listProduct += System.IO.File.ReadAllText(AppContext.BaseDirectory.Replace(@"\Web.Watch", "") + "\\Web.Core\\" + "Service\\EmailTemplate\\ProductTemplate.html")
                    .Replace("{{order.items.title}}", productData.ProductName)
                    .Replace("{{order.items.quantity}}", productData.Qty.ToString())
                    .Replace("{{order.items.price}}", productData.ProductDiscountPrice.ToString());
                }
                body = body.Replace("{{first_name}}", first_name)
                .Replace("{{order_id}}", order_id)
                .Replace("{{status}}", status)
                .Replace("{{order_shipping_address}}", order_shipping_address)
                .Replace("{{order_total_price}}", order_total_price)
                .Replace("{{company_name}}", company_name)
                .Replace("{{company_address}}", company_address)
                .Replace("{{voucher_code}}", voucher_code)
                .Replace("<!--ADD PRODUCT HERE-->", listProduct);

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;
                    var message = new MailMessage();
                    message.From = new MailAddress(smtpUsername);
                    message.To.Add(customer.Email);
                    if (order.Status == 10 || order.Status == 20)
                    {
                        message.Subject = "Xác nhận đơn hàng";
                    }
                    else
                    {
                        if (order.Status == 30)
                        {
                            message.Subject = "Đơn hàng đã bị huỷ";
                        }
                        else
                        {
                            if (order.Status == 40)
                            {
                                message.Subject = "Đã bàn giao cho đơn vị vận chuyển";
                            }
                            else
                            {
                                if (order.Status == 50)
                                {
                                    message.Subject = "Đơn hàng đã hoàn thành";
                                }
                            }
                        }
                    }
                    message.Body = body;
                    message.IsBodyHtml = true;
                    client.Send(message);
                }

                return;
            }
            catch (Exception ex)
            {

            }
        }
        public void SendOTP(string email, string OTP)
        {
            try
            {
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "dangthuanvo1611@gmail.com";
                string smtpPassword = "pwuebwfhtmnujdwt";
                string body = System.IO.File.ReadAllText(AppContext.BaseDirectory.Replace(@"\Web.Watch", "") + "\\Web.Core\\" + "Service\\EmailTemplate\\SendOTPTemplate.html");
                string company_name = "T&TWatch";
                string company_address = "dangthuanvo1611@gmail.com";
                body = body.Replace("{{otp}}", OTP).Replace("{{company_name}}", company_name).Replace("{{company_address}}", company_address);

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;
                    var message = new MailMessage();
                    message.From = new MailAddress(smtpUsername);
                    message.To.Add(email);
                    message.Subject = "Gửi mã OTP";
                    message.Body = body;
                    message.IsBodyHtml = true;
                    client.Send(message);
                }

                return;
            }
            catch (Exception ex)
            {

            }
        }

    }
}