using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NotebookMVCApp.Startup))]
namespace NotebookMVCApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
