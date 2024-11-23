using Core.Entities;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T: BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T?> GetEntityWithSpecificationPattern(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListSpecAsync(ISpecification<T> spec);
        Task<TResult?> GetEntityWithSpecificationPattern<TResult>(ISpecification<T, TResult> spec);
        Task<IReadOnlyList<TResult>> ListSpecAsync<TResult>(ISpecification<T, TResult> spec);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        //Task<bool> SaveAllAsync();
        bool Exist(int id);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}