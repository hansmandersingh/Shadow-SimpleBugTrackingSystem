using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shadow.Startup))]
namespace Shadow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
