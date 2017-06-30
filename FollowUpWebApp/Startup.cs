using System.Linq;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FollowUpWebApp.Startup))]
namespace FollowUpWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            ConfigureAuth(app);
            //app.UseHangfireDashboard();
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                AuthorizationFilters = Enumerable.Empty<IAuthorizationFilter>()
            });
        }
    }
}
