// FileInformation: nyanya/Cat.Domain.Orders/TimelineService.cs
// CreatedTime: 2014/09/16   9:31 AM
// LastUpdatedTime: 2014/09/16   9:32 AM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain.Orders.Database;
using Cat.Domain.Orders.Models;
using Cat.Domain.Orders.ReadModels;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Users.Services;

namespace Cat.Domain.Orders.Services
{
    public class TimelineService : ITimelineInfoService
    {
        protected Func<OrderContext> OrderContextFactory
        {
            get { return () => new OrderContext(); }
        }

        #region ITimelineInfoService Members

        public async Task<Timeline> GetTimelineAsync(string userIdentifier)
        {
            List<TimelineOrder> timelineOrders;
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                timelineOrders = await context.ReadonlyQuery<TimelineOrder>()
                    .Where(o => o.UserIdentifier == userIdentifier).ToListAsync();
            }

            IEnumerable<TimelineNode> orderNodes = timelineOrders.Select(o => new TimelineNode
            {
                Identifier = o.OrderIdentifier,
                Interest = o.Interest,
                Principal = o.Principal,
                Time = o.OrderTime,
                Type = TimelineNodeType.Order
            });

            IEnumerable<TimelineNode> productNodes = timelineOrders.GroupBy(o => o.ProductIdentifier)
                .Select(group => new TimelineNode
                {
                    Identifier = group.Key,
                    Interest = group.Sum(o => o.Interest),
                    Principal = group.Sum(o => o.Principal),
                    Time = group.First().RepaymentDeadline,
                    Type = TimelineNodeType.Product
                });

            DateTime signUpTime = (await (new UserInfoService()).GetUserInfoAsync(userIdentifier)).SignUpTime;
            TimelineNode beginningNode = new TimelineNode
            {
                Identifier = userIdentifier,
                Interest = 0,
                Principal = 0,
                Time = signUpTime,
                Type = TimelineNodeType.Beginning
            };

            TimelineNode todayNode = new TimelineNode
            {
                Identifier = userIdentifier,
                Interest = timelineOrders.Sum(o => o.Interest),
                Principal = timelineOrders.Sum(o => o.Principal),
                Time = DateTime.Today,
                Type = TimelineNodeType.Today
            };

            TimelineNode endNode = new TimelineNode
            {
                Identifier = userIdentifier,
                Interest = timelineOrders.Sum(o => o.Interest),
                Principal = timelineOrders.Sum(o => o.Principal),
                Time = DateTime.MaxValue,
                Type = TimelineNodeType.End
            };

            Timeline timeline = new Timeline();
            timeline.Nodes.Add(beginningNode);
            timeline.Nodes.AddRange(orderNodes);
            timeline.Nodes.Add(todayNode);
            timeline.Nodes.AddRange(productNodes);
            timeline.Nodes.Add(endNode);
            timeline.Nodes = timeline.Nodes.OrderBy(n => n.Time).ToList();

            return timeline;
        }

        #endregion ITimelineInfoService Members
    }
}