using System;

namespace DDD.Sample.Foundation
{
    public interface IUnitOfWork : IDisposable 
    {
        void RegisterUpdate<TEntity>(TEntity entity) where TEntity : class;
        void RegisterInsert<TEntity>(TEntity entity) where TEntity : class;
        void RegisterDelete<TEntity>(TEntity entity) where TEntity : class;
        void Commit();
        void Rollback();

        IQuery CreateQuery();
    }
}
