using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core.Dto;
using Web.Core.Model;

namespace Web.Core.Service
{
    public abstract class GalleryServiceBase : IServiceBase<GalleryDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            using (var context = new MyContext())
            {
                context.Galleries.Remove(context.Galleries.FirstOrDefault(x => x.Id == key));
                context.SaveChanges();
            }
        }

        public virtual List<GalleryDto> GetAll()
        {
            using (var context = new MyContext())
            {
                return context.Galleries.Select(x => new GalleryDto()
                {
                    Id = x.Id,
                    Image = x.Image
                }).ToList();
            }
        }

        public virtual GalleryDto GetById(int key)
        {
            using (var context = new MyContext())
            {
                return context.Galleries
                    .Where(x => x.Id == key)
                    .Select(x => new GalleryDto()
                    {
                        Id = x.Id,
                        Image = x.Image
                    }).FirstOrDefault();
            }
        }

        public virtual GalleryDto Insert(GalleryDto entity)
        {
            Gallery gallery = new Gallery()
            {
                Image = entity.Image
            };
            using (var context = new MyContext())
            {
                context.Galleries.Add(gallery);
                context.SaveChanges();

                return entity;
            }
        }

        public virtual void Update(int key, GalleryDto entity)
        {
            using (var context = new MyContext())
            {
                Gallery gallery = context.Galleries.FirstOrDefault(x => x.Id == key);
                gallery.Image = entity.Image;

                context.SaveChanges();
            }
        }
    }
}