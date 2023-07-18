using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public virtual CustomerDto GetById(string key)
        {
            throw new NotImplementedException();
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