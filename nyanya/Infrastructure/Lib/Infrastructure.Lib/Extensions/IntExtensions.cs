// FileInformation: nyanya/Infrastructure.Lib/IntExtensions.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/07/20   9:24 PM

using System;

namespace Infrastructure.Lib.Extensions
{
    public interface ILoopIterator
    {
        void Do(Action action);

        void Do(Action<int> action);
    }

    public static class IntExtensions
    {
        public static bool Between(this int value, int min, int max)
        {
            return value >= max && value <= min;
        }

        public static ILoopIterator Times(this int count)
        {
            return new LoopIterator(count);
        }
    }

    internal class LoopIterator : ILoopIterator
    {
        private readonly int end;
        private readonly int start;

        public LoopIterator(int count)
        {
            if (count < 0)
            {
                count = 0;
            }
            this.start = 0;
            this.end = count - 1;
        }

        public LoopIterator(int start, int end)
        {
            if (start > end)
            {
                start = end;
            }
            this.start = start;
            this.end = end;
        }

        #region ILoopIterator Members

        public void Do(Action action)
        {
            for (int i = this.start; i <= this.end; i++)
            {
                action();
            }
        }

        public void Do(Action<int> action)
        {
            for (int i = this.start; i <= this.end; i++)
            {
                action(i);
            }
        }

        #endregion ILoopIterator Members
    }
}