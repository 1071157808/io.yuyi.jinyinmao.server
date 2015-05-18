using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(nyanya.Meow.Startup))]

namespace nyanya.Meow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}