using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEventStore.Dispatcher;

namespace Domian.Bus
{
    public interface IEventDispatcher : IDispatchCommits
    {
    }
}