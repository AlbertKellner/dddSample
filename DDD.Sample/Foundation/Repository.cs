using System;
using DDD.Sample.Entityes;
using System.Linq;
using System.Threading;

namespace DDD.Sample.Foundation
{
    public sealed class Repository<TEntity> : IDisposable 
        where TEntity : Entity
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuery _query;
        private readonly IClock _clock;

        public Repository()
        {
            _unitOfWork = UnitOfWorkManager.GetInstance();
            _query = _unitOfWork.CreateQuery();
            _clock = IoC.Resolve<IClock>();
        }

        public TEntity Get(Specification<TEntity> specification)
        {
            var entity = _query.Get(specification);
            return entity;
        }


        public bool Any(Specification<TEntity> specification)
        {
            var any = _query.Any(specification);
            return any;
        }

        public IQueryable<TEntity> GetFrom(Specification<TEntity> specification)
        {
            var entities = _query.GetBySpecification(specification);
            return entities;
        }

        public void Insert(TEntity entity)
        {
            entity.CreatedAt = entity.ModifiedAt = _clock.GetNow();
            entity.CreatedBy = entity.ModifiedBy = Thread.CurrentPrincipal.Identity.Name;

            _unitOfWork.RegisterInsert(entity);
        }

        public void Update(TEntity entity)
        {
            entity.ModifiedAt = _clock.GetNow();
            entity.ModifiedBy = Thread.CurrentPrincipal.Identity.Name;

            _unitOfWork.RegisterUpdate(entity);
        }

        public void Delete(TEntity entity)
        {
            _unitOfWork.RegisterDelete(entity);
        }


        public void Dispose()
        {
            UnitOfWorkManager.Unregister();
            GC.SuppressFinalize(this);
        }
    }
}
