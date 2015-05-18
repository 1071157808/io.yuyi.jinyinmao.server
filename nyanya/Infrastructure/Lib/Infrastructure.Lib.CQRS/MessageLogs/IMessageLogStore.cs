// FileInformation: nyanya/Infrastructure.Lib.CQRS/IMessageLogStore.cs
// CreatedTime: 2014/06/08   6:09 PM
// LastUpdatedTime: 2014/06/08   6:31 PM

using System.Threading.Tasks;
using Infrastructure.Lib.CQRS.Bus;

namespace Infrastructure.Lib.CQRS.MessageLogs
{
    internal interface IMessageLogStore
    {
        Task Create<T>(T message) where T : IMessage;

        Task Delivered<T>(T message, bool successed = true) where T : IMessage;

        Task Handled<T>(T message, bool successed = true) where T : IMessage;
    }
}