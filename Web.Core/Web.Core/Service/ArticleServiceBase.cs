using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core.Dto;
using Web.Core.Model;
using Web.Core.Util;

namespace Web.Core.Service
{
    public abstract class ArticleServiceBase : IServiceBase<ArticleDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            throw new NotImplementedException();
        }

        public virtual List<ArticleDto> GetAll()
        {
            using (var context = new MyContext())
            {
                return context.Articles
                    .Select(x => new ArticleDto()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Alias = x.Alias
                    })
                    .ToList();
            }
        }

        public virtual ArticleDto GetById(int key)
        {
            using (var context = new MyContext())
            {
                return context.Articles
                    .Where(x => x.Id == key)
                   .Select(x => new ArticleDto()
                   {
                       Id = x.Id,
                       Title = x.Title,
                       Alias = x.Alias,
                       Active = x.Active,
                       ShortDescription = x.ShortDescription,
                       Description = x.Description,
                       CategoryId = x.CategoryId,
                       Created = x.Created,
                       Image = x.Image,
                       Index = x.Index,
                       MenuId = x.MenuId,
                       Status = x.Status,
                       Type = x.Type,
                   })
                   .FirstOrDefault();
            }
        }

        public virtual ArticleDto GetByAlias(string alias)
        {
            using (var context = new MyContext())
            {
                return context.Articles
                    .Where(x => x.Alias == alias)
                   .Select(x => new ArticleDto()
                   {
                       Id = x.Id,
                       Title = x.Title,
                       Alias = x.Alias,
                       Active = x.Active,
                       ShortDescription = x.ShortDescription,
                       Description = x.Description,
                       CategoryId = x.CategoryId,
                       Created = x.Created,
                       Image = x.Image,
                       Index = x.Index,
                       MenuId = x.MenuId,
                       Status = x.Status,
                       Type = x.Type,
                   })
                   .FirstOrDefault();
            }
        }

        public virtual ArticleDto Insert(ArticleDto entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(int key, ArticleDto entity)
        {
            using (var context = new MyContext())
            {
                Article article = context.Articles
                    .FirstOrDefault(x => x.Id == key);

                article.Title = entity.Title;
                article.Alias = DataHelper.Unsign(entity.Title) + "-" + entity.Id;
                article.Active = entity.Active;
                article.ShortDescription = entity.ShortDescription;
                article.Description = entity.Description;
                article.CategoryId = entity.CategoryId;
                article.Image = entity.Image;
                article.Index = entity.Index;
                article.MenuId = entity.MenuId;
                article.Status = entity.Status;
                article.Type = entity.Type;

                context.SaveChanges();
            }
        }
    }
}
