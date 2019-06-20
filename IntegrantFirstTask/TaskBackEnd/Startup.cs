using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TaskBackEnd.Startup))]

namespace TaskBackEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}