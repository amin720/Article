using Article.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Article
{
	public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

	        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

	        AuthDbConfig.RegisterAdmin();
		}
    }
}
