using System;
using System.Collections.Generic;
using System.Linq;
using Web.Core.Dto;
using Web.Core.Model;

namespace Web.Core.Service
{
    public abstract class OrderServiceBase : IServiceBase<OrderDto, int>
    {
        SendEmailServiceBase sendEmailServiceBase = new SendEmailServiceBase();
        public virtual void DeleteById(int key, string userSession = null)
        {
            throw new NotImplementedException();
        }

        public virtual List<OrderDto> GetAll()
        {
            using (var context = new MyContext())
            {
                return context.Orders
                    .OrderByDescending(x => x.OrderDate)
                    .Select(x => new OrderDto()
                    {
                        Id = x.Id,
                        Created = x.Created,
                        Customer = new CustomerDto()
                        {
                            Address = x.Customer.Address,
                            Email = x.Customer.Email,
                            FullName = x.Customer.FullName,
                            PhoneNumber = x.Customer.PhoneNumber,
                        },
                        Note = x.Note,
                        CustomerCode = x.CustomerCode,
                        VoucherId = x.VoucherId,
                        OrderDate = x.OrderDate,
                        OrderTime = x.OrderTime,
                        PaymentMethod = x.PaymentMethod,
                        Status = x.Status,
                        TotalAmount = x.TotalAmount,
                    })
                    .ToList();
            }
        }

        public virtual OrderDto GetById(int key)
        {
            using (var context = new MyContext())
            {
                var order = context.Orders.Where((x => x.Id == key)).FirstOrDefault();
                if (order.VoucherId == null)
                {
                    return context.Orders
                        .Where(x => x.Id == key)
                        .Select(x => new OrderDto()
                        {
                            Id = x.Id,
                            Created = x.Created,
                            Customer = new CustomerDto()
                            {
                                Address = x.Customer.Address,
                                Email = x.Customer.Email,
                                FullName = x.Customer.FullName,
                                PhoneNumber = x.Customer.PhoneNumber,
                            },
                            Note = x.Note,
                            OrderDate = x.OrderDate,
                            OrderTime = x.OrderTime,
                            CustomerCode = x.CustomerCode,
                            VoucherId = null,
                            Voucher = new VoucherDto()
                            {
                                VoucherCode = "Không sử dụng mã giảm giá",
                            },
                            PaymentMethod = x.PaymentMethod,
                            Status = x.Status,
                            TotalAmount = x.TotalAmount,
                            OrderDetails = x.OrderDetails.Select(y => new OrderDetailDto()
                            {
                                ProductDiscountPercent = y.ProductDiscountPercent,
                                Id = y.Id,
                                Note = y.Note,
                                OrderId = y.OrderId,
                                ProductId = y.ProductId,
                                ProductDiscountPrice = y.ProductDiscountPrice,
                                ProductImage = y.ProductImage,
                                ProductName = y.ProductName,
                                ProductPrice = y.ProductPrice,
                                Qty = y.Qty,
                                UserDef1 = y.UserDef1,
                                UserDef2 = y.UserDef2,
                                UserDef3 = y.UserDef3,
                                UserDef4 = y.UserDef4,
                                UserDef5 = y.UserDef5
                            }).ToList()
                        })
                        .FirstOrDefault();
                }
                else
                {
                    return context.Orders
                        .Where(x => x.Id == key)
                        .Select(x => new OrderDto()
                        {
                            Id = x.Id,
                            Created = x.Created,
                            Customer = new CustomerDto()
                            {
                                Address = x.Customer.Address,
                                Email = x.Customer.Email,
                                FullName = x.Customer.FullName,
                                PhoneNumber = x.Customer.PhoneNumber,
                            },
                            Note = x.Note,
                            OrderDate = x.OrderDate,
                            OrderTime = x.OrderTime,
                            CustomerCode = x.CustomerCode,
                            VoucherId = x.VoucherId,
                            Voucher = new VoucherDto()
                            {
                                Id = x.Voucher.Id,
                                VoucherCode = x.Voucher.VoucherCode,
                                Description = x.Voucher.Description,
                                Type = x.Voucher.Type,
                                DiscountAmount = x.Voucher.DiscountAmount,
                                IsActive = x.Voucher.IsActive,
                            },
                            PaymentMethod = x.PaymentMethod,
                            Status = x.Status,
                            TotalAmount = x.TotalAmount,
                            OrderDetails = x.OrderDetails.Select(y => new OrderDetailDto()
                            {
                                ProductDiscountPercent = y.ProductDiscountPercent,
                                Id = y.Id,
                                Note = y.Note,
                                OrderId = y.OrderId,
                                ProductId = y.ProductId,
                                ProductDiscountPrice = y.ProductDiscountPrice,
                                ProductImage = y.ProductImage,
                                ProductName = y.ProductName,
                                ProductPrice = y.ProductPrice,
                                Qty = y.Qty,
                                UserDef1 = y.UserDef1,
                                UserDef2 = y.UserDef2,
                                UserDef3 = y.UserDef3,
                                UserDef4 = y.UserDef4,
                                UserDef5 = y.UserDef5
                            }).ToList()
                        })
                        .FirstOrDefault();
                }
            }
        }

        public virtual OrderDto Insert(OrderDto entity)
        {
            using (var context = new MyContext())
            {
                Order order = new Order();
                var customersWithSamePhoneNumber = context.Customers
                    .Where(c => c.PhoneNumber == entity.Customer.PhoneNumber)
                    .ToList();
                if (customersWithSamePhoneNumber.Any())
                {
                    DateTime dateNow = DateTime.Now;
                    order = new Order()
                    {
                        OrderDate = dateNow,
                        Customer = customersWithSamePhoneNumber[0],
                        Note = entity.Note,
                        Status = 10,
                        Created = dateNow,
                        CustomerCode = entity.CustomerCode,
                        VoucherId = entity.VoucherId,

                    };
                }
                else
                {
                    DateTime dateNow = DateTime.Now;
                    order = new Order()
                    {
                        OrderDate = dateNow,
                        Customer = new Customer()
                        {
                            Code = Guid.NewGuid().ToString("N"),
                            Address = entity.Customer.Address,
                            Email = entity.Customer.Email,
                            FullName = entity.Customer.FullName,
                            PhoneNumber = entity.Customer.PhoneNumber
                        },
                        Note = entity.Note,
                        VoucherId = entity.VoucherId,
                        Status = 10,
                        Created = dateNow,
                    };
                }


                List<OrderDetail> orderDetails = new List<OrderDetail>();
                entity.OrderDetails.ForEach(x =>
                {
                    Product product = context.Products.FirstOrDefault(y => y.Id == x.ProductId);

                    orderDetails.Add(new OrderDetail()
                    {
                        ProductId = product.Id,
                        ProductDiscountPercent = product.DiscountPercent,
                        ProductDiscountPrice = product.DiscountPrice,
                        ProductPrice = product.Price,
                        ProductImage = product.Image,
                        ProductName = product.Name,
                        //Attribute = product.ProductAttributes.
                        Qty = x.Qty,
                    });
                });

                order.OrderDetails = orderDetails;
                order.TotalAmount = entity.TotalAmount;

                context.Orders.Add(order);

                context.SaveChanges();

                sendEmailServiceBase.SendEmail(order.Customer, orderDetails, order);

                return entity;
            }
        }

        public virtual void Update(int key, OrderDto entity)
        {
            using (var context = new MyContext())
            {
                Order order = context.Orders
                   .FirstOrDefault(x => x.Id == key);
                var orderdetail = context.OrderDetails.Where(x => x.OrderId == order.Id).ToList();
                var customer = context.Customers.Where(x => x.Code == order.CustomerCode).FirstOrDefault();
                bool flag = false;
                if (order.Status != entity.Status)
                {
                    flag = true;
                }
                order.Status = entity.Status;
                order.Note = entity.Note;

                context.SaveChanges();
                if (flag == true)
                {
                    sendEmailServiceBase.SendEmail(customer, orderdetail, order);
                }
            }
        }
        public virtual List<OrderDto> GetByPhoneNumber(string phonenumber)
        {
            using (var context = new MyContext())
            {
                List<OrderDto> orders = context.Orders
                    .Join(
                        context.Customers,
                        order => order.CustomerCode,
                        customer => customer.Code,
                        (order, customer) => new { Order = order, Customer = customer }
                    )
                    .Where(x => x.Customer.PhoneNumber == phonenumber)
                    .Select(x => new OrderDto
                    {
                        // Map your OrderDto properties here based on the Order and Customer entities
                        Id = x.Order.Id,
                        OrderDate = x.Order.OrderDate,
                        TotalAmount = x.Order.TotalAmount,
                        Customer = new CustomerDto()
                        {
                            FullName = x.Customer.FullName,
                            PhoneNumber = x.Customer.PhoneNumber,
                            Address = x.Customer.Address,


                        },
                        Status = x.Order.Status,
                    })
                    .ToList();

                return orders;
            }
        }
    }
}
