using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(puceAsk_dev1.Startup))]
namespace puceAsk_dev1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
