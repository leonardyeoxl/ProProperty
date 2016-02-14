using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProProperty.Startup))]
namespace ProProperty
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
