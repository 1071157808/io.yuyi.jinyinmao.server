using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Gateway.OldCat
{
    public interface IOldPlatformGateway : IDisposable
    {

        Task UserLoginRequestAsync(OldPlatformParameter parameter);

    }


    public class OldPlatformParameter
    {
        public string AmpAuthToken { get; set; }
        
        public string UserIdentifier { get; set; }

        public string CheckCode { get; set; }
    }
}
