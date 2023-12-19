using System;
using System.Collections.Generic;
using System.Linq;
using Web.Core.Dto;

namespace Web.Core.Service
{
    public abstract class CustomerServiceBase : IServiceBase<CustomerDto, string>
    {
        public virtual void DeleteById(string key, string userSession = null)
        {
            throw new NotImplementedException();
        }

        public virtual List<CustomerDto> GetAll()
        {
            using (var context = new MyContext())
            {
                return context.Customers.Select(x => new CustomerDto()
                {
                    Code = x.Code,
                    PhoneNumber = x.PhoneNumber,
                    FullName = x.FullName,
                    Address = x.Address,
                    Email = x.Email,
                }).ToList();
            }
        }

        public virtual CustomerDto GetById(string key)
        {
            using (var context = new MyContext())
            {
                return context.Customers
                    .Where(x => x.Code == key)
                    .Select(x => new CustomerDto()
                    {
                        Code = x.Code,
                        PhoneNumber = x.PhoneNumber,
                        FullName = x.FullName,
                        Address = x.Address,
                        Email = x.Email,
                    })
                    .FirstOrDefault();
            }
        }
        public virtual CustomerDto GetByEmail(string key)
        {
            using (var context = new MyContext())
            {
                return context.Customers
                    .Where(x => x.Email == key)
                    .Select(x => new CustomerDto()
                    {
                        Code = x.Code,
                        PhoneNumber = x.PhoneNumber,
                        FullName = x.FullName,
                        Address = x.Address,
                        Email = x.Email,
                    })
                    .FirstOrDefault();
            }
        }

        public virtual CustomerDto Insert(CustomerDto entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(string key, CustomerDto entity)
        {
            throw new NotImplementedException();
        }
    }
}