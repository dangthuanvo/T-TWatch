using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core.Dto;

namespace Web.Core.Service
{
    public abstract class AttributeServiceBase : IServiceBase<AttributeDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            throw new NotImplementedException();
        }

        public virtual List<AttributeDto> GetAll()
        {
            using (var context = new MyContext())
            {
                return context.Attributes.Select(x => new AttributeDto()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            }
        }

        public virtual AttributeDto GetById(int key)
        {
            throw new NotImplementedException();
        }

        public virtual AttributeDto Insert(AttributeDto entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(int key, AttributeDto entity)
        {
            throw new NotImplementedException();
        }
    }
}