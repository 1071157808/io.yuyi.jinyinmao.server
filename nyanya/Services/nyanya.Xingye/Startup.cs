using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(nyanya.Xingye.Startup))]

namespace nyanya.Xingye
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}