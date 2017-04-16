using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FollowUpWebApp.Startup))]
namespace FollowUpWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
