// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-03-31  10:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-31  11:33 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Linq;
using GrainInterface;
using Orleans;
using Orleans.Runtime.Configuration;

namespace SiloHosting
{
    /// <summary>
    ///     Orleans test silo host
    /// </summary>
    public class Program
    {
        private static OrleansHostWrapper hostWrapper;

        private static void InitSilo(string[] args)
        {
            hostWrapper = new OrleansHostWrapper(args);

            if (!hostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }

        private static void Main(string[] args)
        {
            // The Orleans silo environment is initialized in its own app domain in order to more
            // closely emulate the distributed situation, when the client and the server cannot
            // pass data via shared memory.
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = args
            });

            GrainClient.Initialize("ClientConfiguration.xml");

            // TODO: once the previous call returns, the silo is up and running.
            //       This is the place your custom logic, for example calling client logic
            //       or initializing an HTTP front end for accepting incoming requests.

            var ids = new[]
            {
                "42783519-d64e-44c9-9c29-399e3afaa625",
                "d694a4e0-1bc3-4c3f-a1ad-ba95103622bc",
                "9a72b0c6-33df-49db-ac05-14316edd332d",
                "6526a751-b9ac-4881-9bfb-836ecce2ca9f",
                "ae4b106f-3c96-464a-b48d-3583ed584b17",
                "b715c40f-d8d2-424d-9618-76afbc0a2a0a",
                "5ad92744-a0b1-487b-a9e7-e6b91e9a9826",
                "e23a55af-217c-4d76-8221-c2b447bf04c8",
                "2eef0ac5-540f-4421-b9a9-79d89400f7ab"
            };

            var e0 = EmployeeFactory.GetGrain(Guid.Parse(ids[0]));
            var e1 = EmployeeFactory.GetGrain(Guid.Parse(ids[1]));
            var e2 = EmployeeFactory.GetGrain(Guid.Parse(ids[2]));
            var e3 = EmployeeFactory.GetGrain(Guid.Parse(ids[3]));
            var e4 = EmployeeFactory.GetGrain(Guid.Parse(ids[4]));

            var m0 = ManagerFactory.GetGrain(Guid.Parse(ids[5]));
            var m1 = ManagerFactory.GetGrain(Guid.Parse(ids[6]));
            var m0e = m0.AsEmployee().Result;
            var m1e = m1.AsEmployee().Result;

            m0e.Promote(10);
            m1e.Promote(11);

            m0.AddDirectReport(e0).Wait();
            m0.AddDirectReport(e1).Wait();
            m0.AddDirectReport(e2).Wait();

            m1.AddDirectReport(m0e).Wait();
            m1.AddDirectReport(e3).Wait();

            m1.AddDirectReport(e4).Wait();

            var reports = m0.GetDirectReports();
            reports.Wait();
            var re = reports.Result;

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(re.Select(r =>
                {
                    var levelTask = r.GetLevel();
                    levelTask.Wait();
                    return levelTask.Result;
                }));
            }

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            hostDomain.DoCallBack(ShutdownSilo);
        }

        private static void ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                hostWrapper.Dispose();
                GC.SuppressFinalize(hostWrapper);
            }
        }
    }
}
