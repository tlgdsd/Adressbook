using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Adressbook.Startup))]
namespace Adressbook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
