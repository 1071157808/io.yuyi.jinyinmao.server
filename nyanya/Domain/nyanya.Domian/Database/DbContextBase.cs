// FileInformation: nyanya/Domian/DbContextBase.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/07/30   3:14 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy;
using Infrastructure.Lib.Utility;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Domian.Database
{
    public abstract class DbContextBase : DbContext
    {
        protected RetryPolicy retryPolicy;

        protected DbContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.retryPolicy = new RetryPolicy(new TransientErrorIgnoreStrategy(), RetryStrategy.NoRetry);
        }

        public void Add<T>(T entity) where T : class
        {
            DbEntityEntry<T> entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
                this.Set<T>().Add(entity);
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            foreach (T entity in entities)
            {
                DbEntityEntry<T> entry = this.Entry(entity);

                if (entry.State == EntityState.Detached)
                    this.Set<T>().Add(entity);
            }
        }

        public void ExecuteAction(Action action)
        {
            Guard.ArgumentNotNull(action, "action");
            this.ExecuteAction(() =>
            {
                action();
                return (object)null;
            });
        }

        public TResult ExecuteAction<TResult>(Func<TResult> func)
        {
            return this.retryPolicy.ExecuteAction(func);
        }

        public Task ExecuteAsync(Func<Task> taskAction)
        {
            return this.retryPolicy.ExecuteAsync(taskAction);
        }

        public Task ExecuteAsync(Func<Task> taskAction, CancellationToken cancellationToken)
        {
            return this.retryPolicy.ExecuteAsync(taskAction, cancellationToken);
        }

        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> taskFunc)
        {
            return this.ExecuteAsync(taskFunc, new CancellationToken());
        }

        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> taskFunc, CancellationToken cancellationToken)
        {
            return this.retryPolicy.ExecuteAsync(taskFunc, cancellationToken);
        }

        public Task<int> ExecuteSaveChangesAsync(CancellationToken cancellationToken)
        {
            return this.ExecuteAsync(() => this.SaveChangesAsync(cancellationToken), cancellationToken);
        }

        public Task<int> ExecuteSaveChangesAsync()
        {
            return this.ExecuteAsync(this.SaveChangesAsync);
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return this.Set<T>();
        }

        public IQueryable<T> ReadonlyQuery<T>() where T : class
        {
            return this.Set<T>().AsNoTracking();
        }

        public void Remove<T>(T entity) where T : class
        {
            this.Entry(entity).State = EntityState.Deleted;

            this.retryPolicy.ExecuteAction(() => this.SaveChanges());
        }

        public Task<int> RemoveAsync<T>(T entity) where T : class
        {
            this.Entry(entity).State = EntityState.Deleted;

            return this.retryPolicy.ExecuteAsync(this.SaveChangesAsync);
        }

        public void Save<T>(T entity) where T : class
        {
            DbEntityEntry<T> entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
                this.Set<T>().Add(entity);
            this.retryPolicy.ExecuteAction(() => this.SaveChanges());
        }

        public Task<int> SaveAsync<T>(T entity) where T : class
        {
            DbEntityEntry<T> entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
                this.Set<T>().Add(entity);

            return this.retryPolicy.ExecuteAsync(this.SaveChangesAsync);
        }

        public Task<int> SaveOrUpdateAsync<T>(T entity, Expression<Func<T, bool>> identifierExpression) where T : class
        {
            DbEntityEntry<T> entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
                this.Set<T>().Add(entity);

            if (this.Set<T>().Any(identifierExpression))
            {
                entry.State = EntityState.Modified;
            }

            return this.retryPolicy.ExecuteAsync(this.SaveChangesAsync);
        }

        public T TryFind<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return this.Set<T>().AsNoTracking().FirstOrDefault(predicate);
        }

        public Task<T> TryFindAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return this.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }
    }
}