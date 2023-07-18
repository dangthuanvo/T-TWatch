using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core.Dto;

namespace Web.Core.Service
{
    public abstract class ReviewServiceBase : IServiceBase<ReviewDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            throw new NotImplementedException();
        }

        public virtual List<ReviewDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual ReviewDto GetById(int key)
        {
            throw new NotImplementedException();
        }

        public virtual ReviewDto Insert(ReviewDto entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(int key, ReviewDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
