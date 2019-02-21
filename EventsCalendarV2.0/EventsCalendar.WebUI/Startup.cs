using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventsCalendar.WebUI.Startup))]
namespace EventsCalendar.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
