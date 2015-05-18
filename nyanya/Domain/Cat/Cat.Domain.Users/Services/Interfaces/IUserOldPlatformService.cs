using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Users.Services.Interfaces
{
    public interface IUserOldPlatformService
    {
        Task SendSignInRequestAsync(string ampAuthToken, string goldCatAuthToken, string userIdentifier);
    }
}
