// FileInformation: nyanya/Infrastructure.Cache.Couchbase/AmpProductCache.cs
// CreatedTime: 2014/03/30   10:17 PM
// LastUpdatedTime: 2014/04/24   2:05 PM

using System;
using System.Configuration;
using Couchbase;
using Couchbase.Configuration;

namespace Infrastructure.Cache.Couchbase
{
    public class AmpProductCache
    {
        private static readonly CouchbaseClient couchbaseClient;
        private readonly string productIdentifier;

        static AmpProductCache()
        {
            CouchbaseClientSection bucketSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/amp_production");

            couchbaseClient = new CouchbaseClient(bucketSection);
        }

        public AmpProductCache(string productIdentifier)
        {
            this.productIdentifier = productIdentifier;
        }

        public int FundedNumber()
        {
            return Convert.ToInt32(couchbaseClient.Get(this.productIdentifier + "paid"));
        }
    }
}