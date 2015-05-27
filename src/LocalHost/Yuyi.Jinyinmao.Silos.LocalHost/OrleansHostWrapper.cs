// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-21  12:14 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  7:17 PM
// ***********************************************************************
// <copyright file="OrleansHostWrapper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Net;
using Orleans.Runtime.Host;

namespace Yuyi.Jinyinmao.Silos.LocalHost
{
    internal sealed class OrleansHostWrapper : IDisposable
    {
        private SiloHost siloHost;

        public OrleansHostWrapper()
        {
            this.Init();
        }

        public bool Debug
        {
            get { return this.siloHost != null && this.siloHost.Debug; }
            set { this.siloHost.Debug = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.siloHost.Dispose();
            this.siloHost = null;
        }

        #endregion IDisposable Members

        public bool Run()
        {
            bool result = false;

            try
            {
                this.siloHost.InitializeOrleansSilo();

                result = this.siloHost.StartOrleansSilo();

                if (result)
                {
                    Console.WriteLine("Successfully started Orleans silo '{0}' as a {1} node.", this.siloHost.Name, this.siloHost.Type);
                }
                else
                {
                    throw new SystemException($"Failed to start Orleans silo '{this.siloHost.Name}' as a {this.siloHost.Type} node.");
                }
            }
            catch (Exception exc)
            {
                this.siloHost.ReportStartupError(exc);
                string msg = $"{exc.GetType().FullName}:\n{exc.Message}\n{exc.StackTrace}";
                Console.WriteLine(msg);
            }

            return result;
        }

        public bool Stop()
        {
            try
            {
                this.siloHost.StopOrleansSilo();

                Console.WriteLine("Orleans silo '{0}' shutdown.", this.siloHost.Name);
            }
            catch (Exception exc)
            {
                this.siloHost.ReportStartupError(exc);
                string msg = $"{exc.GetType().FullName}:\n{exc.Message}\n{exc.StackTrace}";
                Console.WriteLine(msg);
            }

            return true;
        }

        private void Init()
        {
            this.siloHost = new SiloHost(Dns.GetHostName())
            {
                ConfigFileName = "OrleansConfiguration.xml",
                DeploymentId = Guid.NewGuid().ToString(),
                Debug = true
            };
            this.siloHost.LoadOrleansConfig();
        }
    }
}