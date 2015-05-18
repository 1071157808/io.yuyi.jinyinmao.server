using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Database
{
    /// <summary>
    ///   DBContextFactory
    /// </summary>
    public static class DbContextFactory<T> where T : DbContext, new()
    {
        public static T GetInstance()
        {
            return new T();
        }
    }
}
