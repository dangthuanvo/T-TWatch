using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Watch
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string currentAdmin { get; set; }
        public static string OTP { get; set; }
        public static long SC { get; set; }
        public void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
