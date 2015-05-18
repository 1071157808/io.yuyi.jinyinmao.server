using System;

namespace Infrastructure.Lib.Config
{
    public abstract class Config<T>
    {
        public T Data
        {
            get
            {
                if (DateTime.Now >= Expires)
                {
                    this.Payload = this.ReloadData();
                }
                return Payload;
            }
        }

        protected DateTime Expires { get; set; }

        protected T Payload { get; set; }

        protected abstract T ReloadData();
    }
}