// FileInformation: nyanya/Domian.Passport/IUserInfoService.cs
// CreatedTime: 2014/04/02   3:32 PM
// LastUpdatedTime: 2014/04/29   12:58 PM

using System.Security.Principal;
using System.Threading.Tasks;

namespace Domian.Passport.Services.Interfaces
{
    public interface IUserInfoService
    {
        Task<bool> Exist(string cellphone);

        CurrentUser GetCurrentUser(IPrincipal principal);

        Task<CurrentUser> GetCurrentUser(string name);
    }
}