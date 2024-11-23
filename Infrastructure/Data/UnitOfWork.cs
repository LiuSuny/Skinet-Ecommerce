using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data
{
    public class UnitOfWork(StoreContext context) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, Object> _repositories = new();
        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name; //the entity type we are passing ---Repository<TEntity>--

             //next we return an instance our repository entity we pass in the IGenericRepository<TEntity>
             //if we already have it that we are getting or adding to the entity 
            return (IGenericRepository<TEntity>) _repositories.GetOrAdd( type, t => { 
                //if not then we make new one 
                var repositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
                return Activator.CreateInstance(repositoryType, context)
                   //or failed
                   ?? throw new InvalidOperationException($"Could not create repository instance for {t}");
            });
        }
    }
}