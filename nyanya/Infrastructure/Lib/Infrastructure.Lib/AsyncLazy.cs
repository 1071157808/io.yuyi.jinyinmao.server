// FileInformation: nyanya/Infrastructure.Lib/AsyncLazy.cs
// CreatedTime: 2014/06/15   8:00 PM
// LastUpdatedTime: 2014/06/15   8:04 PM

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Infrastructure.Lib
{
    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        public AsyncLazy(Func<T> valueFactory) :
            base(() => Task.Factory.StartNew(valueFactory))
        {
        }

        public AsyncLazy(Func<Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(taskFactory).Unwrap())
        {
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return this.Value.GetAwaiter();
        }
    }
}