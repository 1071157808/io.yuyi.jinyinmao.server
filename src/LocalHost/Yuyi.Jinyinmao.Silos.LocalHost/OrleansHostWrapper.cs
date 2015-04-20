// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-21  12:14 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-21  12:16 AM
// ***********************************************************************
// <copyright file="OrleansHostWrapper.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Net;
using Orleans.Runtime.Host;

namespace Yuyi.Jinyinmao.Silos.LocalHost
{
    internal class OrleansHostWrapper : IDisposable
    {
        private SiloHost siloHost;

        public OrleansHostWrapper(string[] args)
        {
            Init();
        }

        public bool Debug
        {
            get { return siloHost != null && siloHost.Debug; }
            set { siloHost.Debug = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Members

        public bool Run()
        {
            bool result = false;

            try
            {
                siloHost.InitializeOrleansSilo();

                result = siloHost.StartOrleansSilo();

                if (result)
                {
                    Console.WriteLine("Successfully started Orleans silo '{0}' as a {1} node.", siloHost.Name, siloHost.Type);
                }
                else
                {
                    throw new SystemException(string.Format("Failed to start Orleans silo '{0}' as a {1} node.", siloHost.Name, siloHost.Type));
                }
            }
            catch (Exception exc)
            {
                siloHost.ReportStartupError(exc);
                string msg = string.Format("{0}:\n{1}\n{2}", exc.GetType().FullName, exc.Message, exc.StackTrace);
                Console.WriteLine(msg);
            }

            return result;
        }

        public bool Stop()
        {
            try
            {
                siloHost.StopOrleansSilo();

                Console.WriteLine("Orleans silo '{0}' shutdown.", siloHost.Name);
            }
            catch (Exception exc)
            {
                siloHost.ReportStartupError(exc);
                string msg = string.Format("{0}:\n{1}\n{2}", exc.GetType().FullName, exc.Message, exc.StackTrace);
                Console.WriteLine(msg);
            }

            return true;
        }

        protected virtual void Dispose(bool dispose)
        {
            siloHost.Dispose();
            siloHost = null;
        }

        private void Init()
        {
            siloHost = new SiloHost(Dns.GetHostName());
            siloHost.ConfigFileName = "OrleansConfiguration.xml";
            siloHost.DeploymentId = Guid.NewGuid().ToString();
            siloHost.Debug = true;
            siloHost.LoadOrleansConfig();
        }
    }
}
