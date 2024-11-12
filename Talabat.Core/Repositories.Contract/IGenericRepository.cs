using System;
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
        public  Task<IReadOnlyList<T>> GetAllAsync();
        public Task AddAsync(T entity);
        public void DeleteAsync(T entity);
        public void UpdateAsync(T entity);
        public  Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        public Task<int> GetCountForPaginaion(ISpecification<T> spec);
        public Task<T> GetWithSpecAsync(ISpecification<T> spec);
    }
}
