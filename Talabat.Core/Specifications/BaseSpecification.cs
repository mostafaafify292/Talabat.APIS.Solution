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
        public BaseSpecification()
        {
            //Critria = null
        }
        public BaseSpecification(Expression<Func<T, bool>> critriaExpression)
        {
                Critria = critriaExpression;
        }
    }
}
