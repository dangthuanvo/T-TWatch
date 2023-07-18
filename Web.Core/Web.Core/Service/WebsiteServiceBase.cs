using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core.Dto;
using Web.Core.Model;

namespace Web.Core.Service
{
    public abstract class WebsiteServiceBase : IServiceBase<WebsiteDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            throw new NotImplementedException();
        }

        public virtual List<WebsiteDto> GetAll()
        {
            using (var context = new MyContext())
            {
                return context.Websites.Select(x => new WebsiteDto()
                {
                    Address = x.Address,
                    Copyright = x.Copyright,
                    Email = x.Email,
                    Facebook = x.Facebook,
                    Favicon = x.Favicon,
                    Fax = x.Fax,
                    Id = x.Id,
                    Location = x.Location,
                    Logo = x.Logo,
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    TermCondition = x.TermCondition,
                    Twitter = x.Twitter,
                    MetaTitle = x.MetaTitle,
                    MetaContentLanguage = x.MetaContentLanguage,
                    MetaContentType = x.MetaContentType,
                    MetaDescription = x.MetaDescription,
                    MetaRevisitAfter = x.MetaRevisitAfter,
                    MetaRobots = x.MetaRobots,
                    UserDef1 = x.UserDef1,
                    UserDef2 = x.UserDef2,
                    UserDef3 = x.UserDef3,
                    UserDef4 = x.UserDef4,
                    UserDef5 = x.UserDef5,
                    Youtube = x.Youtube
                }).ToList();
            }
        }

        public virtual WebsiteDto GetById(int key)
        {
            throw new NotImplementedException();
        }

        public virtual WebsiteDto Insert(WebsiteDto entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(int key, WebsiteDto entity)
        {
            using (var context = new MyContext())
            {
                Website website = context.Websites.FirstOrDefault(x => x.Id == key);

                website.Address = entity.Address;
                website.Copyright = entity.Copyright;
                website.Email = entity.Email;
                website.Facebook = entity.Facebook;
                website.Favicon = entity.Favicon;
                website.Fax = entity.Fax;
                website.Location = entity.Location;
                website.Logo = entity.Logo;
                website.Name = entity.Name;
                website.PhoneNumber = entity.PhoneNumber;
                website.TermCondition = entity.TermCondition;
                website.Twitter = entity.Twitter;
                website.MetaTitle = entity.MetaTitle;
                website.MetaContentLanguage = entity.MetaContentLanguage;
                website.MetaContentType = entity.MetaContentType;
                website.MetaDescription = entity.MetaDescription;
                website.MetaRevisitAfter = entity.MetaRevisitAfter;
                website.MetaRobots = entity.MetaRobots;
                website.UserDef1 = entity.UserDef1;
                website.UserDef2 = entity.UserDef2;
                website.UserDef3 = entity.UserDef3;
                website.UserDef4 = entity.UserDef4;
                website.UserDef5 = entity.UserDef5;
                website.Youtube = entity.Youtube;

                context.SaveChanges();
            }
        }
    }
}
