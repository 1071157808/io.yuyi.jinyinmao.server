using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domian.Commands
{
    public interface IResultCommandHandler<in T> : ICommandHandler<T> where T : ICommand
    {
        Task<ObjectCommandResult> ResultHandler(T command);
    }
}