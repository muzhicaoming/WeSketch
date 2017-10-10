using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeSketchAPI.Startup))]
namespace WeSketchAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}