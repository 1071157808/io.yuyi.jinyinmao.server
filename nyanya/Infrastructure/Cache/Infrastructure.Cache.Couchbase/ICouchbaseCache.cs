// FileInformation: nyanya/Infrastructure.Cache.Couchbase/ICouchbaseCache.cs
// CreatedTime: 2014/06/23   9:49 AM
// LastUpdatedTime: 2014/06/23   10:05 AM

namespace Infrastructure.Cache.Couchbase
{
    public interface ICouchbaseCache<T>
    {
        ulong CheckValue { get; set; }

        T Value { get; set; }
    }

    public class CouchbaseCache<T> : ICouchbaseCache<T>
    {
        #region ICouchbaseCache<T> Members

        public ulong CheckValue { get; set; }

        public T Value { get; set; }

        #endregion ICouchbaseCache<T> Members
    }
}