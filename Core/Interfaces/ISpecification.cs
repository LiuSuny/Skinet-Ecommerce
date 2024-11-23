using System.Linq.Expressions;

namespace Core.Interfaces
{
    /// <summary>
    /// Specication pattern interface with method to
    /// determine which method we need to support
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
    public interface ISpecification<T>
    {
        //determining what method we need to support vary specification
        Expression<Func<T, bool>>? Criteria{get;}
        Expression<Func<T, object>>? OrderBy{get;}
        Expression<Func<T, object>>? OrderByDescending{get;}
        List<Expression<Func<T, object>>> Includes { get; } 
        List<string>? ThenIncludeString { get; } //this is for theniclude eager loading
        bool IsDistinct {get;}
        int Take {get;}
        int Skip  {get;}
        bool IsPagingEnable {get;}
        IQueryable<T> ApplyCriteria(IQueryable<T> query);
    }
     
     /// <summary>
    /// Specication pattern interface responsible for getting product by type or brand
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
     public interface ISpecification<T, TResult> : ISpecification<T>
    {
        //determining what method we need to support vary specification using select projection
        Expression<Func<T, TResult>>? Select {get;}

    }
}