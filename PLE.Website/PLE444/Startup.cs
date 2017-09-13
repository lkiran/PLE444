using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PLE444.Startup))]
namespace PLE444
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
