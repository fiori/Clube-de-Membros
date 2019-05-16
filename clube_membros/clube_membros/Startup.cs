using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(clube_membros.Startup))]
namespace clube_membros
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
