using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domian.Passport.Services.Interfaces
{
    public interface ITokenDigestProvider
    {
        string GetDigestForUser(string name);
    }
}
