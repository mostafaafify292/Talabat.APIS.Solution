﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Critria { get ; set ; }
        public List<Expression<Func<T, object>>> Includes  { get ; set ; } = new List<Expression<Func<T, object>>> ();
        public Expression<Func<T, object>> OrderBy { get ; set ; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get ; set ; }
        public int Skip { get ; set ; }
        public bool IsPaginationEnabled { get; set; } = false;

        public BaseSpecification()
        {
           
            //Critria = null
        }
        public BaseSpecification(Expression<Func<T,bool>> critriaExpression)
        {
                Critria = critriaExpression;
        }
        public void AddOrderBy(Expression<Func<T, object>> OrderByExperission) //just setter for orderBy
        {
            OrderBy = OrderByExperission;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> OrderByDescExperission) //just setter for orderBy
        {
            OrderByDesc = OrderByDescExperission;
        }
        public void ApplyPagination(int skip , int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;

        }
    }
}
