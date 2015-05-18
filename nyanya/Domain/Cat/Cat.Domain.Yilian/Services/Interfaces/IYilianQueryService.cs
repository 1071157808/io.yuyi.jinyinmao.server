// FileInformation: nyanya/Cat.Domain.Yilian/IYilianQueryService.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/29   6:40 PM

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cat.Domain.Yilian.ReadModels;

namespace Cat.Domain.Yilian.Services.Interfaces
{
    public interface IYilianQueryService
    {
        #region Public Methods

        Task<IList<YilianQueryView>> GetUnCallbackAuthRequestsAsync(DateTime time);


        Task QueryYilian(IEnumerable<YilianQueryView> yilianQueryViews);
        Task<IList<YilianQueryView>> GetUnPaymentResultAsync(DateTime time, IList<string> unResultOrderIds);
        Task QueryYilianPaymentInfo(IEnumerable<YilianQueryView> yilianQueryList);

        #endregion Public Methods
    }
}