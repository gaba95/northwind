using OfficeOpenXml;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Northwind
{
    public class MvcApplication : System.Web.HttpApplication
    {
        [System.Obsolete]
        protected void Application_Start()
        {
            //ExcelPackage.License = new License { Context = LicenseContext.NonCommercial };
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
