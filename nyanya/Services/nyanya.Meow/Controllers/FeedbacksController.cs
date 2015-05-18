// FileInformation: nyanya/nyanya.Meow/FeedbacksController.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:27 PM

using System.Threading.Tasks;
using System.Web.Http;
using Cat.Domain.Meow.Services.Interfaces;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Meow.Models;

namespace Snyanya.Meow.Controllers
{
    /// <summary>
    ///     喵喵反馈
    /// </summary>
    [RoutePrefix("Meow")]
    public class FeedbacksController : ApiControllerBase
    {
        private readonly IFeedbackService feedbackService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FeedbacksController" /> class.
        /// </summary>
        /// <param name="feedbackService">The feedback service.</param>
        public FeedbacksController(IFeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;
        }

        /// <summary>
        ///     创建反馈
        /// </summary>
        /// <param name="request">
        ///     Content[string]: 反馈内容（不能超过2000字）
        /// </param>
        /// <returns>201</returns>
        [Route("Feedbacks")]
        [EmptyParameterFilter("request")]
        [ValidateModelState(Order = 1)]
        public async Task<IHttpActionResult> PostFeedback(FeedbackRequest request)
        {
            await this.feedbackService.AddFeedbackAsync(request.Content, this.CurrentUser.Cellphone);

            return this.OK();
        }
    }
}
