using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ScottASPMVCApp1.Startup))]
namespace ScottASPMVCApp1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
