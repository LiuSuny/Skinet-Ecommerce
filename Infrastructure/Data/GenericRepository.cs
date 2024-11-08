using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        internal DbSet<T> dbSet;
        public GenericRepository(StoreContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
           // _context.Set<T>().Add(entity);
            dbSet.Add(entity);
        }

        public bool Exist(int id)
        {
            //return _context.Set<T>().Any(x => x.Id == id);
           return dbSet.Any(x => x.Id == id);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
           // return await _context.Set<T>().FindAsync(id);
            return await dbSet.FindAsync(id);
        }

        public async Task<T?> GetEntityWithSpecificationPattern(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
            
        }

        public async Task<TResult?> GetEntityWithSpecificationPattern<TResult>(ISpecification<T, TResult> spec)
        {
            return await ApplySpecification<TResult>(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
           // return await _context.Set<T>().ToListAsync();
            return await dbSet.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<IReadOnlyList<TResult>> ListSpecAsync<TResult>(ISpecification<T, TResult> spec)
        {
             return await ApplySpecification<TResult>(spec).ToListAsync();
        }

        public void Remove(T entity)
        {
           // _context.Set<T>().Remove(entity);
             dbSet.Remove(entity);

        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
           //return await dbSet.SaveChangesAsync() > 0
        }

        public void Update(T entity)
        {
            //_context.Set<T>().Attach(entity);
            //_context.Entry(entity).State = EntityState.Modified;

             dbSet.Attach(entity);
             dbSet.Entry(entity).State = EntityState.Modified;
             

        }

        //Helper method for filter, sorting etc
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            //return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
            return SpecificationEvaluator<T>.GetQuery(dbSet.AsQueryable(), spec);

        }

         //Helper method for select
        private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
        {
            //return SpecificationEvaluator<T>.GetQuery<T, TResult>(_context.Set<T>().AsQueryable(), spec)
            return SpecificationEvaluator<T>.GetQuery<T, TResult>(dbSet.AsQueryable(), spec);

        }
    }
}