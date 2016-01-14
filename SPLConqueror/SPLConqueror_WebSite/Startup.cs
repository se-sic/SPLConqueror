using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SPLConqueror_WebSite.Startup))]
namespace SPLConqueror_WebSite
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
