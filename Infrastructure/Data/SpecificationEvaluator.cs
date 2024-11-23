using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    /// <summary>
    /// This class responsible for accessing the db with specification tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        //actual query that is goin to go to the db
        public static IQueryable<T> GetQuery(IQueryable<T> query, 
        ISpecification<T> spec)
        {
            if(spec.Criteria !=null)
            {
                query = query.Where(spec.Criteria); // x => x.Brand ==brand
            }

            if(spec.OrderBy !=null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            
            if(spec.OrderByDescending !=null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            
            if(spec.IsDistinct)
            {
                query = query.Distinct();
            }

            if (spec.IsPagingEnable)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            //aggregate eagar loading to use include 
            query = spec.Includes.Aggregate(query, (currentValue, include)
                => currentValue.Include(include));

            query = spec.ThenIncludeString.Aggregate(query, (currentValue, include)
                  => currentValue.Include(include));



            return query;
        }



        //actual query that is goin to go to the db and because 
        //this is returning a list of string hence we need this method that return Tresult
        public static IQueryable<TResult> GetQuery<Tspec, TResult>(IQueryable<T> query, 
        ISpecification<T, TResult> spec)
        {
            if(spec.Criteria !=null)
            {
                query = query.Where(spec.Criteria); // x => x.Brand ==brand
            }

            if(spec.OrderBy !=null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            
            if(spec.OrderByDescending !=null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            
            //select projection
            var selectQuery = query as IQueryable<TResult>;
            if(spec.Select != null)
            {
                selectQuery = query.Select(spec.Select);
            }

            if(spec.IsDistinct)
            {
                selectQuery = selectQuery?.Distinct();
            }

            if (spec.IsPagingEnable)
            {
                selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take);
            }
            //?? null colision in this case query.Cast<TResult>() is going to executed if slectquery is null
            return selectQuery ?? query.Cast<TResult>();
        }
    }
}