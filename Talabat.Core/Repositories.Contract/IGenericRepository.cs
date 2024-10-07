﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<T> GetAsync(int id);
        public  Task<IEnumerable<T>> GetAllAsync();
        public  Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        public Task<T> GetWithSpecAsync(ISpecification<T> spec);
    }
}
