using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeSketchAPI.Startup))]
namespace WeSketchAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HubConfiguration config = new HubConfiguration();
            config.EnableDetailedErrors = true;
            config.EnableJavaScriptProxies = false;
            app.MapSignalR("/signalr", config);
        }
    }
}