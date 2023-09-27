

using AdventureWorks.WebServices.Repositories;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace AdventureWorks.WebServices
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}