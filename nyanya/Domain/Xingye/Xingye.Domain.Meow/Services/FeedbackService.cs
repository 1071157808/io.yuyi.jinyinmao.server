// FileInformation: nyanya/Xingye.Domain.Meow/FeedbackService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System;
using System.Threading.Tasks;
using Xingye.Domain.Meow.Database;
using Xingye.Domain.Meow.Models;
using Xingye.Domain.Meow.Services.Interfaces;

namespace Xingye.Domain.Meow.Services
{
    public class FeedbackService : IFeedbackService
    {
        #region IFeedbackService Members

        public async Task AddFeedbackAsync(string content, string cellphone = "")
        {
            using (MeowContext context = new MeowContext())
            {
                await context.SaveAsync(new Feedback
                {
                    Cellphone = cellphone,
                    Content = content,
                    Time = DateTime.Now
                });
            }
        }

        #endregion IFeedbackService Members
    }
}