using Microsoft.EntityFrameworkCore;
using Noon.Core.Entities;
using Noon.Core.Specifications;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Repository.Spec
{
    public static class SpecificationEvaluator<TEntity>where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if(spec.Criteria  != null) 
                query=query.Where(spec.Criteria);

            if(spec.OrderBy != null)
                query=query.OrderBy(spec.OrderBy);

            if(spec.OrderByDesc != null)
                query=query.OrderByDescending(spec.OrderByDesc);



            if(spec.IsPaginationEnabled)
                query=query.Skip(spec.Skip).Take(spec.Take);

            query=spec.Includes.Aggregate(query,(currentquery,expressionquery)=>currentquery.Include(expressionquery));


            return query;

        }

    }
}
