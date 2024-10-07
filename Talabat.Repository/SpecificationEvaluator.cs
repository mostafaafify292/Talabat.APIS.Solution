using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> innerquery , ISpecification<TEntity> spec)
        {
            var query = innerquery; //_dbcontext.Set<Product>()
            if (spec.Critria is not null)
            {
                query = query.Where(spec.Critria);//_dbcontext.Set<Product>().Where(P => P.Id == id)
            }
            //query = _dbcontext.Set<Product>().Where(P => P.Id == id)
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            return query;

        }
    }
}
