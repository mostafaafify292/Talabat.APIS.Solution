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

            //where()
            if (spec.Critria is not null)
            {
                query = query.Where(spec.Critria);//_dbcontext.Set<Product>().Where(P => P.Id == id)
            }


            //orderby()
            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
             
            //orderbydesc()
            else if (spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            //Take().Skip()
            if (spec.IsPaginationEnabled == true)
            {
                
                    query = query.Skip(spec.Skip).Take(spec.Take);
                

            }
            //include()
            //query = _dbcontext.Set<Product>().Where(P => P.Id == id)
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            return query;



        }
    }
}
