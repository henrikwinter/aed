using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AedWeb.Startup))]
namespace AedWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
