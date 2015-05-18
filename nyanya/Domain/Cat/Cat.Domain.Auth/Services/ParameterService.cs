using System.Data.Entity;
using Cat.Domain.Auth.Database;
using Cat.Domain.Auth.Models;
using Cat.Domain.Auth.Services.Interfaces;
using Domian.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Cache.Couchbase;

namespace Cat.Domain.Auth.Services
{
    public class ParameterService : IParameterService
    {
        private readonly DbContextFactory dbContextFactory;

        public ParameterService(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<string> GetValue(string name)
        {
            var cache = ParameterCache.GetByName(name);
            if (cache != null && !string.IsNullOrEmpty(cache.Value))
            {
                return cache.Value;
            }
            using (AuthContext context = this.dbContextFactory.Create<AuthContext>())
            {
                var parmeter =  await context.ReadonlyQuery<Parameter>().FirstOrDefaultAsync(x => x.Name.Equals(name));
                ParameterCache.SetCache(name, parmeter.Value);
                return parmeter.Value;
            }
        }

    }
}
