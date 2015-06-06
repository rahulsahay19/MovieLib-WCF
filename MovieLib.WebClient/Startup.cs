using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieLib.WebClient.Startup))]
namespace MovieLib.WebClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
