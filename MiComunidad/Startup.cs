using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MiComunidad.Startup))]
namespace MiComunidad
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
