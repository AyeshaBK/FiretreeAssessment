using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FiretreeAssessment.Startup))]
namespace FiretreeAssessment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
