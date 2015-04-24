// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-21  12:14 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  1:52 AM
// ***********************************************************************
// <copyright file="OrleansHostWrapper.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Net;
using Orleans.Runtime.Host;
using Yuyi.Jinyinmao.Domain;

namespace Yuyi.Jinyinmao.Silos.LocalHost
{
    internal class OrleansHostWrapper : IDisposable
    {
        private SiloHost siloHost;

        public OrleansHostWrapper(string[] args)
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
            this.Dispose(true);
        }

        #endregion IDisposable Members

        public bool Run()
        {
            bool result = false;

            try
            {
                SiloClusterConfig.CheckConfig();
                this.siloHost.InitializeOrleansSilo();

                result = this.siloHost.StartOrleansSilo();

                if (result)
                {
                    Console.WriteLine("Successfully started Orleans silo '{0}' as a {1} node.", this.siloHost.Name, this.siloHost.Type);
                }
                else
                {
                    throw new SystemException(string.Format("Failed to start Orleans silo '{0}' as a {1} node.", this.siloHost.Name, this.siloHost.Type));
                }
            }
            catch (Exception exc)
            {
                this.siloHost.ReportStartupError(exc);
                string msg = string.Format("{0}:\n{1}\n{2}", exc.GetType().FullName, exc.Message, exc.StackTrace);
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
                string msg = string.Format("{0}:\n{1}\n{2}", exc.GetType().FullName, exc.Message, exc.StackTrace);
                Console.WriteLine(msg);
            }

            return true;
        }

        protected virtual void Dispose(bool dispose)
        {
            this.siloHost.Dispose();
            this.siloHost = null;
        }

        private void Init()
        {
            this.siloHost = new SiloHost(Dns.GetHostName());
            this.siloHost.ConfigFileName = "OrleansConfiguration.xml";
            this.siloHost.DeploymentId = Guid.NewGuid().ToString();
            this.siloHost.Debug = true;
            this.siloHost.LoadOrleansConfig();
        }
    }
}
