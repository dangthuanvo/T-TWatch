using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Watch
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Search",
                url: "tim-kiem",
                defaults: new { controller = "Home", action = "Search" }
            );
            routes.MapRoute(
                name: "BaiViet",
                url: "bai-viet/{alias}",
                defaults: new { controller = "Home", action = "Article" }
            );

            routes.MapRoute(
                name: "ShoppingCart",
                url: "gio-hang",
                defaults: new { controller = "Home", action = "ShoppingCart" }
            );

            routes.MapRoute(
                name: "Buy",
                url: "mua-hang/{id}",
                defaults: new { controller = "Home", action = "Buy" }
            );

            routes.MapRoute(
                name: "Menu",
                url: "{alias}",
                defaults: new { controller = "Home", action = "Category" }
            );

            routes.MapRoute(
                name: "Product",
                url: "san-pham/{alias}",
                defaults: new { controller = "Home", action = "ProductDetail" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
