// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : LogAspect.cs
// Created          : 2015-08-16  21:05
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-16  21:41
// ***********************************************************************
// <copyright file="LogAspect.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Linq;
using System.Text;
using Moe.Lib;
using PostSharp.Aspects;
using Yuyi.Jinyinmao.Packages;

namespace Yuyi.Jinyinmao.Log
{
    /// <summary>
    ///     LogExceptionAspect.
    /// </summary>
    [Serializable]
    public class LogExceptionAspect : OnExceptionAspect
    {
        /// <summary>
        ///     Method executed <b>after</b> the body of methods to which this aspect is applied,
        ///     in case that the method resulted with an exception (i.e., in a <c>catch</c> block).
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override void OnException(MethodExecutionArgs args)
        {
            JinyinmaoException exception = new JinyinmaoException(args.Exception.Message, args.Exception);

            StringBuilder argumentsStringBuilder = new StringBuilder();
            foreach (object argument in args.Arguments)
            {
                argumentsStringBuilder.Append($"{argument.GetType()},{argument.ToJson()};");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(exception.EventId);
            sb.Append(Environment.NewLine);
            sb.Append(exception.Message);
            sb.Append(Environment.NewLine);
            sb.Append(args.Method.Name);
            if (args.Method.GetGenericArguments().Any())
            {
                sb.Append(Environment.NewLine);
                sb.Append(args.Method.GetGenericArguments().Select(a => a.ToString()).Join(","));
            }
            if (argumentsStringBuilder.Length > 0)
            {
                sb.Append(Environment.NewLine);
                sb.Append(argumentsStringBuilder);
            }
            sb.Append(Environment.NewLine);
            sb.Append(exception.GetExceptionString());

            LogManager.GetExceptionLogger().Fatal(sb.ToString());

            args.FlowBehavior = FlowBehavior.ThrowException;
            args.Exception = exception;
        }
    }
}