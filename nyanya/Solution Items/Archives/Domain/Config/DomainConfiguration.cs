// FileInformation: nyanya/Domain/DomainConfiguration.cs
// CreatedTime: 2014/06/24   11:13 AM
// LastUpdatedTime: 2014/06/24   1:55 PM

using Infrastructure.Lib.CQRS.Log;
using Ninject;

namespace Domain.Config
{
    /// <summary>
    ///     需要重构为Service Container
    /// </summary>
    public static class DomainConfiguration
    {
        public static IKernel Kernel { get; set; }

        public static ILogger Logger { get; set; }
    }
}