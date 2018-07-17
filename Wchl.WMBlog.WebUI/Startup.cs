using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Wchl.WMBlog.WebUI.Startup))]
namespace Wchl.WMBlog.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
