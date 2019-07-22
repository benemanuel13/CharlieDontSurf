using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CharlieDontSurf.Startup))]
namespace CharlieDontSurf
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
