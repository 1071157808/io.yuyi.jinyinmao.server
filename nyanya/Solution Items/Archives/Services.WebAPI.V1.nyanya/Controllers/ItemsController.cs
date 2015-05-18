// FileInformation: nyanya/Services.WebAPI.V1.nyanya/ItemsController.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/29   2:44 PM

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Services.WebAPI.Common.Controller;
using Services.WebAPI.Common.Dtos;
using Services.WebAPI.Common.Filters;
using Services.WebAPI.Common.RequestCommands;
using Services.WebAPI.V1.nyanya.Filters;
using Services.WebAPI.V1.nyanya.Models;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     道具相关接口
    /// </summary>
    public class ItemsController : ApiControllerBase
    {
        /// <summary>
        ///     为指定的订单使用道具
        /// </summary>
        /// <param name="request">
        ///     ItemId[int]: 道具Id
        ///     OrderIdentifier[string]: 订单Identifier
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     {"V": "1.23"}(保留2位小数)
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "道具不存在"
        ///     "订单不存在"
        ///     "道具使用失败"
        /// </returns>
        [Route("Items/ConsumeByOrder")]
        [TokenAuthorize]
        [EmptyParameterFilter("request", Order = 1)]
        [ValidateModelState(Order = 2)]
        public IHttpActionResult ConsumeByOrder(ItemConsumeRequest request)
        {
            return this.BadRequest("道具不存在");
            //CurrentUser user = this.CurrentUser;

            //OHPItem item = await this.meowContext.OHPItems.AsNoTracking().FirstOrDefaultAsync(i => i.Id == request.ItemId && i.OwnerGuid == user.Identifier && !i.IsUsed && DateTime.Now < i.Expires);
            //if (item == null)
            //{
            //    return this.BadRequest("道具不存在");
            //}

            //OrderWithPI order = await this.orderContext.OrderWithPIs.AsNoTracking().FirstOrDefaultAsync(i => i.OrderId == request.OrderId && i.UserGuid == user.Identifier && !i.ItemId.HasValue);
            //if (order == null)
            //{
            //    return this.BadRequest("订单不存在");
            //}

            //decimal? extraInterest = await item.ConsumeByOrderAsync(order);
            //if (extraInterest.HasValue)
            //{
            //    // 2014-05-14 14:36 使用道具后，返回增加的收益,保留2位小数
            //    return this.Ok(new { V = decimal.Round(extraInterest.Value, 2) });
            //}
            //return this.BadRequest("道具使用失败");
        }

        /// <summary>
        ///     Distributes the FHP item.(非业务接口)
        /// </summary>
        /// <param name="userGuid">The user unique identifier.</param>
        /// <returns>
        ///     200/400("该用户不存在。")
        /// </returns>
        [HttpGet]
        [Route("Items/{userGuid}")]
        [TokenAuthorize(AllowLocal = true, Roles = "Administrator")]
        public IHttpActionResult DistributeItem(string userGuid)
        {
            //if (String.IsNullOrEmpty(userGuid) || (await this.passportContext.Users.CountAsync(u => u.Uuid == userGuid) == 0))
            //{
            //    return this.BadRequest("该用户不存在。");
            //}

            //await this.itemService.DistributeOHPItemAsync(userGuid, DateTime.Now.Date.AddDays(ApplicationConfig.Item.OHPItem.ValidityDuration));

            //this.itemService.SetNewItemFlag(userGuid);

            return this.OK();
        }

        /// <summary>
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-Items_PageIndex_PageSize.jpg"@}获取道具列表 - M @{/a@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-Items_PageIndex_PageSize.jpg"@}获取道具列表 - APP@{/a@}
        /// </summary>
        /// <param name="cmd">
        ///     PageIndex(int >= 1): 页码
        ///     PageSize(20 >= int >= 1): 一页节点数量
        /// </param>
        /// <returns>
        ///     @{h3@} 分页列表: @{/h3@}
        ///     HasNextPage: 是否有下一页
        ///     PageIndex: 页码
        ///     PageSize: 一页节点数量
        ///     TotalCount: 所有的节点数量
        ///     TotalPageCount: 总页数
        ///     Items:节点列表
        ///     -- Id: 道具Id
        ///     -- InterestDescription: 道具描述
        ///     -- Expires: 过期时间
        ///     -- HasExpired[bool]: 是否过期
        ///     -- ImageSrc: 图片地址，绝对地址 http://m.jinyinmao.com.cn/public/ItemImages/V1/{size}/001.jpg ({size}整体替换为需要的大小)
        ///     -- -- -- -- -- -- -- -- --例如 http://m.jinyinmao.com.cn/public/ItemImages/V1/720/001.jpg ({size}整体替换为需要的大小)
        ///     -- Title: 道具名称
        /// </returns>
        [Route("Items")]
        [TokenAuthorize]
        [EmptyParameterFilter("cmd", Order = 1)]
        [ValidateModelState(Order = 2)]
        [ResponseType(typeof(PaginatedDto<ItemDto>))]
        public async Task<IHttpActionResult> GetItems(PaginatedRequestCommand cmd)
        {
            return this.Ok(PaginatedDto<ItemDto>.Empty);
            //CurrentUser user = this.CurrentUser;

            //PaginatedList<Item> items = await this.itemService.GetPaginatedItemsAsync(cmd.PageIndex, cmd.PageSize, user.Identifier);

            //PaginatedDto<ItemDto> itemDtos = items.ToPaginatedDto(i => i.ToItemDto());

            //this.itemService.RemoveNewItemFlag(user.Identifier);

            //return this.Ok(itemDtos);
        }

        /// <summary>
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-Orders_PageIndex_PageSize.jpg"@}获取道具数量和道具是否有更新 - M @{/a@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-Orders_PageIndex_PageSize.jpg"@}获取道具数量和道具是否有更新 - APP@{/a@}
        /// </summary>
        /// <returns>
        ///     Count: 道具数量
        ///     New[bool]: 是否有新道具
        /// </returns>
        [Route("Items/Count")]
        [TokenAuthorize]
        public IHttpActionResult GetItemsCount()
        {
            return this.Ok(new { Count = 0, New = false });
            //CurrentUser user = this.CurrentUser;

            //int count = await this.meowContext.Items.AsNoTracking().CountAsync(i => i.OwnerGuid == user.Identifier && !i.IsUsed);

            //bool newArrival = this.itemService.CheckNewArrival(user.Identifier);

            //return this.Ok(new { Count = count, New = newArrival });
        }

        /// <summary>
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-Items_PageIndex_PageSize.jpg"@}获取可以使用的道具列表 - M @{/a@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-Items_PageIndex_PageSize.jpg"@}获取可以使用道具的列表 - APP@{/a@}
        /// </summary>
        /// <param name="cmd">
        ///     OrderId(int >= 1): 道具Id
        ///     PageIndex(int >= 1): 页码
        ///     PageSize(20 >= int >= 1): 一页节点数量
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     @{h3@} 分页列表: @{/h3@}
        ///     HasNextPage: 是否有下一页
        ///     PageIndex: 页码
        ///     PageSize: 一页节点数量
        ///     TotalCount: 所有的节点数量
        ///     TotalPageCount: 总页数
        ///     -- Id: 道具Id
        ///     -- InterestDescription: 道具描述
        ///     -- Expires: 过期时间
        ///     -- HasExpired[bool]: 是否过期
        ///     -- ImageSrc: 图片地址，绝对地址 http://m.jinyinmao.com.cn/public/ItemImages/V1/{size}/001.jpg ({size}整体替换为需要的大小)
        ///     -- -- -- -- -- -- -- -- --例如 http://m.jinyinmao.com.cn/public/ItemImages/V1/720/001.jpg ({size}整体替换为需要的大小)
        ///     -- Title: 道具名称
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "订单不存在。"
        ///     "订单不能使用该道具。"
        /// </returns>
        [Route("Items/Useable")]
        [TokenAuthorize]
        [EmptyParameterFilter("cmd", Order = 1)]
        [ValidateModelState(Order = 2)]
        [ResponseType(typeof(PaginatedDto<ItemDto>))]
        public IHttpActionResult GetUseableItems([FromUri] UseableItemsRequest cmd)
        {
            return this.Ok(PaginatedDto<ItemDto>.Empty);
            //CurrentUser user = this.CurrentUser;

            //Domain.Order.Models.Order order = await this.orderContext.Orders.AsNoTracking().FirstOrDefaultAsync(i => i.Id == cmd.OrderId);
            //if (order == null)
            //{
            //    return this.BadRequest("订单不存在。");
            //}

            ////HACK: 这里认为投资中订单，即已付款或者起息中的订单，都可以使用道具，只适合于只有100本金券的业务情况下使用
            //if (!(order.Status == OrderStatus.InterestAccruing || order.Status == OrderStatus.Paid))
            //{
            //    return this.BadRequest("订单不能使用该道具。");
            //}

            //PaginatedList<Item> items = await this.itemService.GetPaginatedUseableItemsAsync(cmd.PageIndex, cmd.PageSize, user.Identifier);

            //PaginatedDto<ItemDto> itemDtos = items.ToPaginatedDto(i => i.ToItemDto());

            //return this.Ok(itemDtos);
        }

        /// <summary>
        ///     Removes the item.
        /// </summary>
        /// <param name="id">道具ID</param>
        /// <returns>200 No Content</returns>
        [HttpGet]
        [Route("Items/Remove/{id}")]
        [Route("Items/Remove")]
        [TokenAuthorize]
        public IHttpActionResult RemoveItem(int id)
        {
            //CurrentUser user = this.CurrentUser;

            //Task.Run(() => this.itemService.RemoveItem(id, user.Identifier));

            return this.OK();
        }
    }
}