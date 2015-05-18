// FileInformation: nyanya/Xingye.Domain.Yilian/YilianQueryService.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/29   6:32 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xingye.Domain.Yilian.Database;
using Xingye.Domain.Yilian.Models;
using Xingye.Domain.Yilian.ReadModels;
using Xingye.Domain.Yilian.Services.Interfaces;

namespace Xingye.Domain.Yilian.Services
{
    public class YilianQueryService : IYilianQueryService
    {
        #region Public Methods

        public async Task<IList<YilianQueryView>> GetUnCallbackAuthRequestsAsync(DateTime time)
        {
            //DateTime time = DateTime.Now.AddMinutes(-13);
            using (YilianContext context = new YilianContext())
            {
                return await context.ReadonlyQuery<YilianQueryView>()
                    .Where(p => p.CallbackResult == null && p.GatewayResult != null && p.GatewayResult == true && p.QueryResult == null && p.CreateTime < time && !p.IsPayment).ToArrayAsync();
            }
        }

        public async Task<IList<YilianQueryView>> GetUnPaymentListAsync(DateTime time)
        {
            //DateTime time = DateTime.Now.AddMinutes(-8);
            using (YilianContext context = new YilianContext())
            {
                return await context.ReadonlyQuery<YilianQueryView>()
                    .Where(p => p.CallbackResult == null && p.GatewayResult != null && p.GatewayResult == true && p.QueryResult == null && p.CreateTime < time && p.IsPayment).Take(50).ToArrayAsync();
            }
        }

        public async Task QueryYilian(IEnumerable<YilianQueryView> yilianQueryViews)
        {
            foreach (YilianQueryView yilianQueryView in yilianQueryViews)
            {
                YilianQueryAuthRequest info = new YilianQueryAuthRequest(yilianQueryView.RequestIdentifier);

                await info.SendQueryAuthRequestAsync(yilianQueryView.RequestIdentifier, yilianQueryView.UserIdentifier, yilianQueryView.SequenceNo);
            }
        }

        public async Task QueryYilianPaymentInfo(IEnumerable<YilianQueryView> yilianQueryList)
        {
            foreach (YilianQueryView yilianQueryView in yilianQueryList)
            {
                YilianQueryAuthRequest info = new YilianQueryAuthRequest(yilianQueryView.RequestIdentifier);

                await info.SendQueryPaymentRequestAsync(yilianQueryView.RequestIdentifier, yilianQueryView.OrderIdentifier, yilianQueryView.UserIdentifier, yilianQueryView.SequenceNo);
            }
        }

        #endregion Public Methods
    }
}