// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  2:51 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  11:47 AM
// ***********************************************************************
// <copyright file="JBYProductIssuedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     JBYProductIssuedProcessor.
    /// </summary>
    public class JBYProductIssuedProcessor : EventProcessor<JBYProductIssued>, IJBYProductIssuedProcessor
    {
    }
}