using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(nyanya.Cat.Startup))]

namespace nyanya.Cat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
