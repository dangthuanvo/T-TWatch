using System;
using System.Collections.Generic;
using System.Linq;
using Web.Core.Dto;
using Web.Core.Model;

namespace Web.Core.Service
{
    public abstract class ReviewServiceBase : IServiceBase<ReviewDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            using (var context = new MyContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Review review = context.Reviews.FirstOrDefault(x => x.Id == key);

                    context.Reviews.Remove(review);

                    context.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public virtual List<ReviewDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual ReviewDto GetById(int key)
        {
            using (var context = new MyContext())
            {
                return context.Reviews
                    .Where(x => x.Id == key)
                    .Select(x => new ReviewDto()
                    {
                        Id = x.Id,
                        Star = x.Star,
                        Content = x.Content,
                        ProductId = x.ProductId,
                        Active = x.Active,
                        Created = x.Created,
                        CustomerCode = x.CustomerCode,
                        CustomerName = x.CustomerName,
                    })
                    .FirstOrDefault();
            }
        }

        public virtual ReviewDto Insert(ReviewDto entity)
        {
            using (var context = new MyContext())
            {
                Review review = new Review()
                {
                    Star = entity.Star,
                    Content = entity.Content,
                    ProductId = entity.ProductId,
                    Active = entity.Active,
                    Created = entity.Created,
                    CustomerCode = entity.CustomerCode,
                    CustomerName = entity.CustomerName,
                };
                context.Reviews.Add(review);
                context.SaveChanges();

                return entity;
            }
        }

        public virtual void Update(int key, ReviewDto entity)
        {
            using (var context = new MyContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Review review = context.Reviews.FirstOrDefault(x => x.Id == key);
                    review.Active = entity.Active;
                    context.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public virtual List<ReviewDto> GetByProductID(int key)
        {
            using (var context = new MyContext())
            {
                return context.Reviews.Where(x => x.ProductId == key).Where(x => x.Active == 1).Select(x => new ReviewDto()
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Star = x.Star,
                    Content = x.Content,
                    CustomerCode = x.CustomerCode,
                    Created = x.Created,
                    CustomerName = x.CustomerName,
                    Active = x.Active,
                }).ToList();

            }
        }
    }
}
