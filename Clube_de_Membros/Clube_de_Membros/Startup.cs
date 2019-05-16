using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Clube_de_Membros.Startup))]
namespace Clube_de_Membros
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
