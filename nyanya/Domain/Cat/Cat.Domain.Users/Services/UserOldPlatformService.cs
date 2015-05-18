using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Cat.Domain.Users.ReadModels;
using Cat.Domain.Users.Services.Interfaces;
using Infrastructure.Gateway.OldCat;

namespace Cat.Domain.Users.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserOldPlatformService : IUserOldPlatformService
    {
        public async Task SendSignInRequestAsync(string ampAuthToken, string goldCatAuthToken, string userIdentifier)
        {
            IUserInfoService userInfoService = new UserInfoService();
            UserInfo userInfo = await userInfoService.GetUserInfoAsync(userIdentifier);
            if (userInfo != null && userInfo.HasYSBInfo)
            {
                OldPlatformParameter oldPlatformParameter = BuildOldPlatformParameter(ampAuthToken,
                userIdentifier);
                IOldPlatformGateway oldPlatformGateway = new OldPlatformGateway();
                await oldPlatformGateway.UserLoginRequestAsync(oldPlatformParameter);
            }
            
        }

        private OldPlatformParameter BuildOldPlatformParameter(string ampAuthToken, string userIdentifier)
        {
            return new OldPlatformParameter()
            {
                AmpAuthToken = ampAuthToken,
                
                UserIdentifier = userIdentifier,
                CheckCode = FormsAuthentication.HashPasswordForStoringInConfigFile(userIdentifier+ampAuthToken+ "new_old_jinyinmao", "MD5").ToLower()
            };
        }
    }


}
