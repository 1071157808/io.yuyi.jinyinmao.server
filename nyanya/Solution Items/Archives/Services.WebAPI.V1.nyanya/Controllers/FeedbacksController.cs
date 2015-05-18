// FileInformation: nyanya/Services.WebAPI.V1.nyanya/FeedbacksController.cs
// CreatedTime: 2014/07/29   10:36 AM
// LastUpdatedTime: 2014/08/11   12:39 PM

using System.Threading.Tasks;
using System.Web.Http;
using Cqrs.Domain.Meow.Services.Interfaces;
using Services.WebAPI.Common.Controller;
using Services.WebAPI.Common.Filters;
using Services.WebAPI.V1.nyanya.Models;

namespace Services.WebAPI.V1.nyanya.Controllers.Meow
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
        ///     Content[string]: 反馈内容（不能超过200字）
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