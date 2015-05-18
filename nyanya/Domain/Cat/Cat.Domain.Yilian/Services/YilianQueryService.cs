// FileInformation: nyanya/Cat.Domain.Yilian/YilianQueryService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/24   6:21 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain.Orders.Database;
using Cat.Domain.Orders.ReadModels;
using Cat.Domain.Yilian.Database;
using Cat.Domain.Yilian.Models;
using Cat.Domain.Yilian.ReadModels;
using Cat.Domain.Yilian.Services.Interfaces;

namespace Cat.Domain.Yilian.Services
{
    public class YilianQueryService : IYilianQueryService
    {
        #region IYilianQueryService Members

        public async Task<IList<YilianQueryView>> GetUnCallbackAuthRequestsAsync(DateTime time)
        {
            //DateTime time = DateTime.Now.AddMinutes(-13);
            using (YilianContext context = new YilianContext())
            {
                return await context.ReadonlyQuery<YilianQueryView>()
                    .Where(p => p.CallbackResult == null && p.GatewayResult != null && p.GatewayResult == true && p.QueryResult == null && p.CreateTime < time && !p.IsPayment).ToArrayAsync();
            }
        }

        public async Task<IList<YilianQueryView>> GetUnPaymentResultAsync(DateTime time,IList<string> unResultOrderIds )
        {
            //DateTime time = DateTime.Now.AddMinutes(-8);
            IList<string> noResultOrderIds;
            using (YilianContext context = new YilianContext())
            {
                return await context.ReadonlyQuery<YilianQueryView>()
                    .Where(p => unResultOrderIds.Contains(p.OrderIdentifier) && p.GatewayResult != null && p.GatewayResult == true && p.IsPayment).Take(50).ToArrayAsync();
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

        #endregion IYilianQueryService Members
    }
}