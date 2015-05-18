// FileInformation: nyanya/Domain/DbContextBase.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/06/19   11:15 AM

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy;
using Infrastructure.Lib.Utility;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Domain.Database
{
    public abstract class DbContextBase : DbContext
    {
        protected RetryPolicy retryPolicy;

        protected DbContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.retryPolicy = new RetryPolicy(new TransientErrorIgnoreStrategy(), RetryStrategy.NoRetry);
        }

        public void ExecuteAction(Action action)
        {
            this.retryPolicy.ExecuteAction(action);
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
            return this.retryPolicy.ExecuteAsync(taskAction, new CancellationToken());
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

        public Task<int> ExecuteSaveChangesAsync()
        {
            return this.retryPolicy.ExecuteAsync(this.SaveChangesAsync);
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return this.Set<T>();
        }

        public IQueryable<T> ReadonlyQuery<T>() where T : class
        {
            return this.Set<T>().AsNoTracking();
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

        public void SaveOrUpdate<T>(T entity) where T : class
        {
            this.retryPolicy.ExecuteAction(() => this.Set<T>().AddOrUpdate(entity));
        }

        public void SaveOrUpdate<T>(T entity, Expression<Func<T, object>> identifierExpression) where T : class
        {
            this.retryPolicy.ExecuteAction(() => this.Set<T>().AddOrUpdate(identifierExpression, entity));
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
    }
}