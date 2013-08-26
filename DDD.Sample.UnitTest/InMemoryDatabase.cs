using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Rhino.Mocks;
using DDD.Sample.Entityes;
using DDD.Sample.Foundation;

namespace DDD.Sample.UnitTest
{
    public class InMemoryDatabase
    {
        public DateTime NowFake;
        private static readonly ConcurrentDictionary<int, Context> Contexts = new ConcurrentDictionary<int, Context>();

        enum Command
        {
            Insert, Update, Delete
        }

        struct Context
        {
            public IUnitOfWork UnitOfWork;
            public IQuery Query;
            public IClock Clock;
        }

        static InMemoryDatabase()
        {
            IoC.Register<IClock, IClock>(() => Contexts[Thread.CurrentThread.ManagedThreadId].Clock);
            IoC.Register<IUnitOfWork, IUnitOfWork>(() => Contexts[Thread.CurrentThread.ManagedThreadId].UnitOfWork);
            IoC.Register<IQuery, IQuery>(() => Contexts[Thread.CurrentThread.ManagedThreadId].Query);
        }

        public InMemoryDatabase(DateTime now, params Assembly[] assemblies)
        {
            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            var clock = MockRepository.GenerateMock<IClock>();
            var query = MockRepository.GenerateMock<IQuery>();

            unitOfWork.Expect(u => u.CreateQuery()).WhenCalled(_ =>
                _.ReturnValue = IoC.Resolve<IQuery>()
                );
            NowFake = now;
            clock.Expect(c => c.GetNow()).WhenCalled(_ => _.ReturnValue = NowFake).Return(default(DateTime));
            unitOfWork.Expect(u => u.Dispose()).WhenCalled(a => UnitOfWorkManager.Unregister());
            
            Contexts.TryAdd(Thread.CurrentThread.ManagedThreadId,
                new Context
                    {
                        Clock = clock,
                        UnitOfWork = unitOfWork,
                        Query = query 
                    });

            foreach (var assembly in assemblies)
                ConfigureAllRepositoryEntities(assembly, unitOfWork, query);
        }

        private void ConfigureAllRepositoryEntities(Assembly domain, IUnitOfWork unitOfWork, IQuery query)
        {
            foreach (var methodEntity in from entity in domain.GetTypes()
                                         where !entity.IsAbstract && typeof (Entity).IsAssignableFrom(entity)
                                         let methodInfo = typeof (InMemoryDatabase).GetMethod("ConfigureRepository")
                                         select methodInfo.MakeGenericMethod(entity))
            {
                methodEntity.Invoke(this, new object[] {unitOfWork, query});
            }
        }

        public void ConfigureRepository<TEntity>(IUnitOfWork unitOfWork, IQuery query)
            where TEntity : Entity
        {
            ICollection<KeyValuePair<Command, TEntity>> registerCommands = new List<KeyValuePair<Command, TEntity>>();
            ICollection<TEntity> database = new List<TEntity>();

            unitOfWork.Expect(u => u.RegisterInsert<TEntity>(null))
                .IgnoreArguments()
                .WhenCalled(_ => Insert(_, registerCommands, database));
            unitOfWork.Expect(u => u.RegisterUpdate<TEntity>(null))
                .IgnoreArguments()
                .WhenCalled(_ => Update(_, registerCommands));
            unitOfWork.Expect(u => u.RegisterDelete<TEntity>(null))
                .IgnoreArguments()
                .WhenCalled(_ => Delete(_, registerCommands, database));
            unitOfWork.Expect(u => u.Commit())
                .WhenCalled(_ => Commit(registerCommands));
            unitOfWork.Expect(u => u.Rollback())
                .WhenCalled(_ => Rollback(registerCommands, database));

            query.Expect(q => q.Get<TEntity>(null))
                .IgnoreArguments()
                .WhenCalled(_ => Get(_, database))
                .Return(default(TEntity));

            query.Expect(q => q.GetBySpecification<TEntity>(null))
                .IgnoreArguments()
                .WhenCalled(_ => GetBySpecification(_, database))
                .Return(Enumerable.Empty<TEntity>().AsQueryable());

            query.Expect(q => q.GetAll<TEntity>())
                .IgnoreArguments()
                .WhenCalled(_ => _.ReturnValue = database)
                .Return(Enumerable.Empty<TEntity>().AsQueryable());

            query.Expect(q => q.Any<TEntity>(null))
                .IgnoreArguments()
                .WhenCalled(_ => Any(_, database))
                .Return(false);
        }

        private static void Get<TEntity>(MethodInvocation method,
            IEnumerable<TEntity> database)
        {
            var specification = (Specification<TEntity>)method.Arguments[0];
            var query = specification.SatisfiedBy.Compile();
            method.ReturnValue = database.SingleOrDefault(query);
        }

        private static void GetBySpecification<TEntity>(MethodInvocation method,
            IEnumerable<TEntity> database)
        {
            var specification = (Specification<TEntity>)method.Arguments[0];
            var query = specification.SatisfiedBy.Compile();
            method.ReturnValue = database.Where(query).AsQueryable();
        }

        private static void Any<TEntity>(MethodInvocation method,
            IEnumerable<TEntity> database)
        {
            var specification = (Specification<TEntity>)method.Arguments[0];
            var query = specification.SatisfiedBy.Compile();
            method.ReturnValue = database.Any(query);
        }

        private static void Insert<TEntity>(MethodInvocation method, 
            ICollection<KeyValuePair<Command, TEntity>> registerCommands,
            ICollection<TEntity> database)
        {
            var entity = (TEntity) method.Arguments[0];
            registerCommands.Add(new KeyValuePair<Command, TEntity>(Command.Insert, entity));
            database.Add(entity);
        }

        private static void Update<TEntity>(MethodInvocation method,
            ICollection<KeyValuePair<Command, TEntity>> registerCommands)
        {
            var entity = (TEntity)method.Arguments[0];
            registerCommands.Add(new KeyValuePair<Command, TEntity>(Command.Update, entity));
        }

        private static void Delete<TEntity>(MethodInvocation method,
            ICollection<KeyValuePair<Command, TEntity>> registerCommands,
            ICollection<TEntity> database)
        {
            var entity = (TEntity)method.Arguments[0];
            registerCommands.Add(new KeyValuePair<Command, TEntity>(Command.Delete, entity));
            database.Remove(entity);
        }

        private static void Commit<TEntity>(ICollection<KeyValuePair<Command, TEntity>> registerCommands)
        {
            registerCommands.Clear();
        }

        private static void Rollback<TEntity>(
            IEnumerable<KeyValuePair<Command, TEntity>> registerCommands,
            ICollection<TEntity> database)
        {
            foreach(var command in registerCommands)
            {
                switch (command.Key)
                {
                    case Command.Delete:
                        database.Add(command.Value);
                        break;
                    case Command.Insert:
                        database.Add(command.Value);
                        break;
                }
            }
        }

    }
}
