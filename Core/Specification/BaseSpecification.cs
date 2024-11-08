using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Specification
{
    public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) 
    : ISpecification<T>
    {
        protected BaseSpecification() : this(null){}
        //Passing in the expression instances
        public Expression<Func<T, bool>>? Criteria => criteria;

        public Expression<Func<T, object>>? OrderBy {get; private set;}

        public Expression<Func<T, object>>? OrderByDescending {get; private set;}

        public bool IsDistinct {get; private set;}

        public int Take {get; private set;}

        public int Skip {get; private set;}

        public bool IsPagingEnable {get; private set;}

        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            if(Criteria !=null)
            {
                query = query.Where(Criteria);
            }
            return query;
        }

        //in order to get hold of this method from derived class
        protected void AddOrderBy(Expression<Func<T, object>>? orderByExpression)
        {
            OrderBy =  orderByExpression;
        }

         protected void AddOrderByDescending(Expression<Func<T, object>>? orderByDescendingExpression)
        {
            OrderByDescending =  orderByDescendingExpression;
        }

         protected void ApplyDistinct()
        {
            IsDistinct = true;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnable = true;
        }

    }


    //class responible for brand and type projection
    public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria)
   : BaseSpecification<T>(criteria), ISpecification<T, TResult>
    {
        protected BaseSpecification() : this(null){}

        public Expression<Func<T, TResult>>? Select {get; private set;}
        
         protected void AddSelectByBrandAndType(Expression<Func<T, TResult>>? selectExpression)
        {
            Select =  selectExpression;
        }
    }

}