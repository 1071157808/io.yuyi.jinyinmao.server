// FileInformation: nyanya/Infrastructure.Lib.CQRS/IConfigSource.cs
// CreatedTime: 2014/06/04   4:36 PM
// LastUpdatedTime: 2014/06/04   5:05 PM

using Infrastructure.Lib.CQRS.Log;

namespace Infrastructure.Lib.CQRS.Config
{
    /// <summary>
    ///     Represents that the implemented classes are configuration sources.
    /// </summary>
    public interface IConfigSource
    {
        /// <summary>
        ///     Gets the instance of <see cref="Infrastructure.Lib.CQRS.Config.ConfigSection" /> class.
        /// </summary>
        ConfigSection Config { get; }

        /// <summary>
        /// Gets the instance of <see cref="Infrastructure.Lib.CQRS.Config.LogSection" /> class.
        /// </summary>
        LogSection Loggers { get; }
    }
}