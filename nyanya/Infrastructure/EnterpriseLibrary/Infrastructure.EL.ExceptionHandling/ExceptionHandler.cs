using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using NLog;

namespace ExceptionHandler
{
    public static class ExceptionHandler
    {
        private static Logger logger = LogManager.GetLogger("GlobalExceptionLogger");
        public static ExceptionManager GetExMgr()
        {
            var policies = new List<ExceptionPolicyDefinition>();

            var logAndWrap = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                         new NLogExceptionHandler(logger),
                         new WrapHandler("出错了，请联系管理员。", typeof(Exception))
                     })
            };

            var logAndReplace = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                        new NLogExceptionHandler(logger),
                        new ReplaceHandler("出错了，请联系管理员。", typeof(Exception))
                     })
            };

            var replace = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new ReplaceHandler("出错了，请联系管理员。", typeof(Exception))
                     })
            };

            var wrap = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new WrapHandler("出错了，请联系管理员。", typeof(Exception))
                     })
            };

            policies.Add(new ExceptionPolicyDefinition("LogAndWrap", logAndWrap));
            policies.Add(new ExceptionPolicyDefinition("LogAndReplace", logAndReplace));
            policies.Add(new ExceptionPolicyDefinition("Replace", replace));
            policies.Add(new ExceptionPolicyDefinition("Wrap", wrap));
            return new ExceptionManager(policies);
        }
    }

    public class NLogExceptionHandler : IExceptionHandler
    {
        private Logger _logger;
        public NLogExceptionHandler(Logger logger)
        {
            this._logger = logger;
        }
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            _logger.Error("", exception);
            return exception;
        }
    }
}
