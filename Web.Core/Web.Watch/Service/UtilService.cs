using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Dto;

namespace Web.Watch.Service
{
    public class UtilService
    {
        public static WebsiteDto GetWebsite()
        {
            return new WebsiteService().GetAll().FirstOrDefault();
        }

        public static List<MenuDto> GetMenus()
        {
            return new MenuService().GetAllActive();
        }

        public static List<MenuDto> GetMenusHomePage()
        {
            return new MenuService().GetAllShowHomePage();
        }

        public static List<ArticleDto> GetArticle()
        {
            return new ArticleService().GetAll();
        }
    }
}