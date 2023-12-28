using System;
using System.Collections.Generic;
using System.Linq;
using Web.Core.Dto;

namespace Web.Core.Service
{
    public class ReportServiceBase
    {
        public virtual ReportHighlight GetHighlight()
        {
            DateTime dateNow = DateTime.Now;

            using (var context = new MyContext())
            {
                ReportHighlight highlight = new ReportHighlight();
                highlight.TotalNewOrder = context.Orders
                    .Where(x => x.OrderDate.Day == dateNow.Day &&
                                x.OrderDate.Month == dateNow.Month &&
                                x.OrderDate.Year == dateNow.Year
                                && x.Status == 50)
                    .Count();

                highlight.DailySales = context.Orders
                    .Where(x => x.OrderDate.Day == dateNow.Day &&
                                x.OrderDate.Month == dateNow.Month &&
                                x.OrderDate.Year == dateNow.Year)
                    .Sum(x => x.TotalAmount == null ? 0 : x.TotalAmount) ?? 0;

                highlight.TotalOrder = context.Orders
                   .Where(x => x.OrderDate.Month == dateNow.Month &&
                               x.OrderDate.Year == dateNow.Year)
                   .Count();

                highlight.SalesRevenue = context.Orders
                   .Where(x => x.OrderDate.Month == dateNow.Month &&
                               x.OrderDate.Year == dateNow.Year
                               && x.Status == 50)
                    .Sum(x => x.TotalAmount == null ? 0 : x.TotalAmount) ?? 0;

                highlight.Revenues = new List<double>();

                for (int i = 1; i < 13; i++)
                {
                    double total = context.Orders
                                .Where(x => x.OrderDate.Month == i &&
                                            x.OrderDate.Year == dateNow.Year
                                            && x.Status == 50)
                                .Sum(x => x.TotalAmount == null ? 0 : x.TotalAmount) ?? 0;

                    highlight.Revenues.Add(total);
                }

                highlight.TenOrderLastest = context.Orders
                    .OrderByDescending(x => x.OrderDate)
                    .Select(x => new OrderDto()
                    {
                        Id = x.Id,
                        Customer = new CustomerDto()
                        {
                            FullName = x.Customer.FullName
                        },
                        OrderDate = x.OrderDate,
                        TotalAmount = x.TotalAmount
                    })
                    .Take(10)
                    .ToList();

                return highlight;
            }
        }

        public virtual List<OrderDto> GetGeneralReport(DateTime? startDate, DateTime? toDate)
        {
            using (var context = new MyContext())
            {
                var query = context.Orders.AsQueryable();
                DateTime tDate = new DateTime();
                DateTime sDate = new DateTime();

                if (startDate.HasValue && toDate.HasValue)
                {
                    tDate = toDate.Value.AddDays(1).Date;
                    sDate = startDate.Value.Date;
                }

                return query
                    .Where(x => x.Status == 50 && x.OrderDate >= sDate && x.OrderDate < tDate)
                    .OrderByDescending(x => x.OrderDate)
                    .Select(x => new OrderDto()
                    {
                        Id = x.Id,
                        Customer = new CustomerDto()
                        {
                            FullName = x.Customer.FullName,
                            Address = x.Customer.Address,
                            Code = x.Customer.Code,
                            Email = x.Customer.Email,
                            PhoneNumber = x.Customer.PhoneNumber
                        },
                        Created = x.Created,
                        CustomerCode = x.CustomerCode,
                        Note = x.Note,
                        OrderDate = x.OrderDate,
                        OrderTime = x.OrderTime,
                        PaymentMethod = x.PaymentMethod,
                        Status = x.Status,
                        TotalAmount = x.TotalAmount
                    })
                    .ToList();
            }
        }

        public virtual List<double> GetRevenueReport(DateTime date)
        {
            List<double> datas = new List<double>();
            using (var context = new MyContext())
            {
                for (int i = 1; i < 13; i++)
                {
                    double total = context.Orders
                                .Where(x => x.OrderDate.Month == i &&
                                            x.OrderDate.Year == date.Year
                                            && x.Status == 50)
                                .Sum(x => x.TotalAmount == null ? 0 : x.TotalAmount) ?? 0;

                    datas.Add(total);
                }

                return datas;
            }
        }
    }
}
