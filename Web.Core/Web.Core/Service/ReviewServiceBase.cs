using System;
using System.Collections.Generic;
using System.Linq;
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

        public virtual List<ReviewDto> GetByProductID(int key, ReviewDto entity)
        {
            using (var context = new MyContext())
            {
                return context.Reviews.Where(x => x.ProductId == key).Select(x => new ReviewDto()
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Star = x.Star,
                    Content = x.Content,
                    CustomerCode = x.CustomerCode,
                    Active = x.Active,
                }).ToList();

            }
        }
    }
}
