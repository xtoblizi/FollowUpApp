using FollowUpWebApp.Schedullar;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hangfire;
using Hangfire.SqlServer;
using GlobalConfiguration = System.Web.Http.GlobalConfiguration;

namespace FollowUpWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        readonly EventJob _eventJob = new EventJob();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Jobschedular.Start();
            //Jobschedular2.Start();
            JobStorage.Current = new SqlServerStorage("DefaultConnection");
            RecurringJob.AddOrUpdate(() => _eventJob.Execute(), "10 00 * * *");
        }
    }
}
