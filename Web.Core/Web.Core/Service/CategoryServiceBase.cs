using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core.Dto;

namespace Web.Core.Service
{
    public abstract class CategoryServiceBase : IServiceBase<CategoryDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            throw new NotImplementedException();
        }

        public virtual List<CategoryDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual CategoryDto GetById(int key)
        {
            throw new NotImplementedException();
        }

        public virtual CategoryDto Insert(CategoryDto entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(int key, CategoryDto entity)
        {
            throw new NotImplementedException();
        }
    }
}