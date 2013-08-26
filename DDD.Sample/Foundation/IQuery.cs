using System.Linq;
using DDD.Sample.Entityes;

namespace DDD.Sample.Foundation
{
    public interface IQuery
    {
        bool Any<TEntity>(Specification<TEntity> specification) where TEntity : Entity;
        TEntity Get<TEntity>(Specification<TEntity> specification) where TEntity : Entity;
        IQueryable<TEntity> GetBySpecification<TEntity>(Specification<TEntity> specification) where TEntity : Entity;
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : Entity;
    }
}
