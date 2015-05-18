using Cat.Domain.Orders.Models;
using Cat.Domain.Orders.Services.DTO;
using Cat.Domain.Products.Models;
using Domian.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cat.Domain.Orders.Services.Interfaces
{
    public interface IZCBOrderService
    {
        Task<IPaginatedDto<ZCBBill>> GetZCBBillListAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10);

        Task<IPaginatedDto<ZCBUserBill>> GetZCBUserBillListAsync(string userIdentifier,DateTime startTime,DateTime endTime, int pageIndex = 1, int pageSize = 10);

        Task<ZCBUserRemainRedeemInfo> CheckRedeemPrincipalAsync(string userIdentifier, string productIdentifier);

        Task<int> CheckRedeemPrincipalCount(string userIdentifier);

        Task<decimal> GetUserTodayInvesting(string userIdentifier);

        Task<decimal> GetUnRedeemPrincipal(string userIdentifier);

        Task<IList<ZCBUser>> GetActiveZcbUsers();

        Task SetYesterDayInterest(ZCBUser zcbUser, IList<ZCBHistory> yieldHistories);

        Task CheckRedeemApplication(ZCBUser zcbUser, IList<ZCBHistory> yieldHistories);

        Task<ZCBUser> GetZCBUserAsync(string userIdentifier);
    }
}