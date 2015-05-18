// FileInformation: nyanya/Cat.Domain.Meow/FeedbackService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System;
using System.Threading.Tasks;
using Cat.Domain.Meow.Database;
using Cat.Domain.Meow.Models;
using Cat.Domain.Meow.Services.Interfaces;

namespace Cat.Domain.Meow.Services
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