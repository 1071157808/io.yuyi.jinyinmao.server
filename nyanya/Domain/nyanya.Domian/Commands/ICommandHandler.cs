// FileInformation: nyanya/Domian/ICommandHandler.cs
// CreatedTime: 2014/07/10   12:07 AM
// LastUpdatedTime: 2014/07/10   12:08 AM

using System.Threading.Tasks;

namespace Domian.Commands
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        Task Handler(T command);
    }
}