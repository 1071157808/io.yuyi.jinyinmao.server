// FileInformation: nyanya/Services.WebAPI.V1.nyanya/AdminTaskController.cs
// CreatedTime: 2014/05/22 1:56 AM
// LastUpdatedTime: 2014/05/22 1:30 PM

using System.Web.Http;
using Newtonsoft.Json.Linq;
using Services.WebAPI.V1.nyanya.App_Start;
using Services.WebAPI.V1.nyanya.Filters;

namespace Services.WebAPI.V1.nyanya.Controllers.DevAdmin
{
    /// <summary>
    /// AdminTaskController （管理接口，非业务接口）
    /// </summary>
    [RoutePrefix("DevAdmin/Task")]
    public class AdminTaskController : ApiController
    {
        /// <summary>
        /// Cancels the specified task name.
        /// </summary>
        /// <param name="taskName">Name of the task.</param>
        [HttpGet]
        [Route("Cancel/{taskName}")]
        [TokenAuthorize(AllowLocal = true, Roles = "Administrator")]
        public IHttpActionResult Cancel(string taskName)
        {
            JObject before = AdminTaskManager.TaskManager.GetTaskStatus(taskName);
            bool result = AdminTaskManager.TaskManager.Cancel(taskName);
            JObject after = AdminTaskManager.TaskManager.GetTaskStatus(taskName);

            return this.Ok(new { Result = result, Before = before, After = after });
        }

        /// <summary>
        /// Informations of the AdminTaskManager.
        /// </summary>
        [HttpGet]
        [Route("Info")]
        [TokenAuthorize(AllowLocal = true, Roles = "Administrator")]
        public IHttpActionResult Info()
        {
            return this.Ok(AdminTaskManager.TaskManager);
        }

        /// <summary>
        /// Runs the specified task name.
        /// </summary>
        /// <param name="taskName">Name of the task.</param>
        [HttpGet]
        [Route("Run/{taskName}")]
        [TokenAuthorize(AllowLocal = true, Roles = "Administrator")]
        public IHttpActionResult Run(string taskName)
        {
            JObject before = AdminTaskManager.TaskManager.GetTaskStatus(taskName);
            bool result = AdminTaskManager.TaskManager.RunTask(taskName);
            JObject after = AdminTaskManager.TaskManager.GetTaskStatus(taskName);

            return this.Ok(new { Result = result, Before = before, After = after });
        }

        /// <summary>
        /// Statuses the specified task name.
        /// </summary>
        /// <param name="taskName">Name of the task.</param>
        [HttpGet]
        [Route("Status/{taskName}")]
        [TokenAuthorize(AllowLocal = true, Roles = "Administrator")]
        public IHttpActionResult Status(string taskName)
        {
            return this.Ok(AdminTaskManager.TaskManager.GetTaskStatus(taskName));
        }
    }
}