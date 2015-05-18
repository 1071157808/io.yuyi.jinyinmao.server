// FileInformation: nyanya/Cat.Domain.Meow/FeedbackService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System.Threading.Tasks;
using Cat.Domain.Meow.Services.DTO;
using Cat.Domain.Meow.Services.Interfaces;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Lib.Extensions;

namespace Cat.Domain.Meow.Services
{
    public class UpgradeService : IUpgradeService
    {

        public async Task<UpgradeResult> GetUpgradeAsync(string channel, string source, string version)
        {
            MeowUpgradeCacheModel model = MeowUpgradeCache.GetMeowUpgradeCache(channel);

            UpgradeResult res = new UpgradeResult { status = false, url = "", version = "", message = "" };
            if (model.IsNotNull())
            {
                res.status = true;
                res.url = model.Url;
                res.version = model.MaxVersion;
                res.message = model.Message;
            }

            return res;
        }

        public async Task<UpgradeExResult> GetUpgradeExAsync(string channel, string source, string version)
        {
            MeowUpgradeCacheModel model = MeowUpgradeCache.GetMeowUpgradeCache(channel);

            UpgradeExResult res = new UpgradeExResult { status = 0, url = "", version = "", message = "" };
            if (model.IsNotNull())
            {
                if (version.ToFloat() < model.MustVersion.ToFloat())
                {
                    res.status = 2;
                }
                else if (version.ToFloat() < model.MaxVersion.ToFloat())
                {
                    res.status = 1;
                }
                else
                {
                    res.status = 0;
                }

                res.url = model.Url;
                res.version = model.MaxVersion;
                res.message = model.Message;
            }

            return res;
        }
    }
}