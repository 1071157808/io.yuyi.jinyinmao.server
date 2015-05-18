// FileInformation: nyanya/Infrastructure.Lib.CQRS/ICommandLogStore.cs
// CreatedTime: 2014/07/01   2:04 PM
// LastUpdatedTime: 2014/07/01   2:04 PM

using System;
using System.Threading.Tasks;

namespace Infrastructure.Lib.CQRS.MessageLogs
{
    public interface ICommandLogStore
    {
        Task Create(CommandLog log);

        Task Handled(Guid commandId, bool successed = true, Exception exception = null);
    }
}