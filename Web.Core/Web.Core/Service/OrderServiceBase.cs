using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core.Dto;
using Web.Core.Model;

namespace Web.Core.Service
{
    public abstract class OrderServiceBase : IServiceBase<OrderDto, int>
    {
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

        public virtual OrderDto Insert(OrderDto entity)
        {
            using (var context = new MyContext())
            {
                DateTime dateNow = DateTime.Now;
                Order order = new Order()
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
                    Status = 10,
                    Created = dateNow,
                };

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
                order.TotalAmount = orderDetails.Sum(x => x.Qty * x.ProductDiscountPrice);

                context.Orders.Add(order);

                context.SaveChanges();

                return entity;
            }
        }

        public virtual void Update(int key, OrderDto entity)
        {
            using (var context = new MyContext())
            {
                Order order = context.Orders
                   .FirstOrDefault(x => x.Id == key);

                order.Status = entity.Status;
                order.Note = entity.Note;

                context.SaveChanges();
            }
        }
    }
}
