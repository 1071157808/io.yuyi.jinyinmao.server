using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Yuyi.Jinyinmao.UrlTransfer.Controllers
{
    /// <summary>
    /// HomeController.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Route("index")]
        public ActionResult Index()
        {
            return this.Redirect("/activities/index.html" + this.GetParameter());
        }

        /// <summary>
        /// Index2s this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Route("index2")]
        public ActionResult Index2()
        {
            return this.Redirect("/activities/index2.html" + this.GetParameter());
        }

        /// <summary>
        /// Index3s this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Route("index3")]
        public ActionResult Index3()
        {
            return this.Redirect("/activities/index3.html" + this.GetParameter());
        }

        /// <summary>
        /// Index4s this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Route("index4")]
        public ActionResult Index4()
        {
            return this.Redirect("/activities/index4.html" + this.GetParameter());
        }

        /// <summary>
        /// Index5s this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Route("index5")]
        public ActionResult Index5()
        {
            return this.Redirect("/activities/index5.html" + this.GetParameter());
        }

        /// <summary>
        /// Kongbais this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Route("index_kongbai")]
        public ActionResult Kongbai()
        {
            return this.Redirect("/activities/index_kongbai.html" + this.GetParameter());
        }

        /// <summary>
        /// Txes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Route("tx")]
        public ActionResult Tx()
        {
            return this.Redirect("/activities/tx.html" + this.GetParameter());
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetParameter()
        {
            List<string> list = this.Request.QueryString.AllKeys.Select(x => x + "=" + this.Request.QueryString[x]).ToList();
            string parameter = list.Any() ? "?" + string.Join("&", list) : string.Empty;
            return parameter;
        }
    }
}