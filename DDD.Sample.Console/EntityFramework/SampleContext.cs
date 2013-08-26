using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Sample.Foundation;
using System.Data.Entity;
using DDD.Sample.Entityes;

namespace DDD.Sample.Console.EntityFramework
{
    public class SampleContext : DbContext, IUnitOfWork, IQuery
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInWarehouse> ProductsInWarehouse { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SystemParameters> SystemParameters { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.CodeCatalog);
            modelBuilder.Entity<Product>()
                .Property(p => p.CodeCatalog)
                .IsRequired();
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired();
            modelBuilder.Entity<Product>()
                .Ignore(p => p.LastPurchase);


            modelBuilder.Entity<Warehouse>()
                .HasKey(p => p.Code);
            modelBuilder.Entity<Warehouse>()
                .Property(p => p.Code)
                .IsRequired();

            //modelBuilder.Entity<ProductInWarehouse>()
            //    .HasRequired(p => p.Product)
            //    .WithMany(p => p.ProductsInWarehouse)
            //    .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<ProductInWarehouse>()
                .HasRequired(p => p.Warehouse)
                .WithMany(p => p.ProductsInWarehouse)
                .WillCascadeOnDelete(false); 

            modelBuilder.Entity<Supplier>()
                .HasKey(p => p.Code );

            modelBuilder.Entity<Supplier>()
                .Property(p => p.Code)
                .IsRequired();


            modelBuilder.Entity<SystemParameters>()
                .HasKey(p => p.Code );

            modelBuilder.Entity<SystemParameters>()
                .Property(p => p.Code)
                .IsRequired();

            //modelBuilder.Entity<Purchase>()
            //    .HasRequired(p => p.Product)
            //    .WithMany(p => p.Purchases)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Purchase>()
            //    .HasRequired(p => p.Supplier)
            //    .WithMany(p => p.Purchases)
            //    .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public void RegisterUpdate<TEntity>(TEntity entity)
             where TEntity : class
        {
            Set<TEntity>().Attach(entity);
            Entry(entity).State = EntityState.Modified;
        }

        public void RegisterInsert<TEntity>(TEntity entity)
             where TEntity : class
        {
            Set<TEntity>().Add(entity);
        }

        public void RegisterDelete<TEntity>(TEntity entity)
             where TEntity : class
        {
            if (Entry(entity).State == EntityState.Detached)
            {
                Set<TEntity>().Attach(entity);
            }
            Set<TEntity>().Remove(entity);
        }

        public void Commit()
        {
            this.SaveChanges();
        }

        public void Rollback()
        {
            var changedEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Modified))
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Added))
            {
                entry.State = EntityState.Detached;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Deleted))
            {
                entry.State = EntityState.Unchanged;
            }
        }

        public IQuery CreateQuery()
        {
            return this;
        }

        public bool Any<TEntity>(Specification<TEntity> specification) where TEntity : Entityes.Entity
        {
            IQueryable<TEntity> query = Set<TEntity>();
            return query.Any();
        }

        public TEntity Get<TEntity>(Specification<TEntity> specification) where TEntity : Entityes.Entity
        {
            IQueryable<TEntity> query = Set<TEntity>();
            return query.SingleOrDefault(specification.SatisfiedBy);
        }

        public IQueryable<TEntity> GetBySpecification<TEntity>(Specification<TEntity> specification) where TEntity : Entityes.Entity
        {
            IQueryable<TEntity> query = Set<TEntity>();
            return query.Where(specification.SatisfiedBy);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : Entityes.Entity
        {
            IQueryable<TEntity> query = Set<TEntity>();
            return query;
        }

    }
}
