using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbcontext;

        public GenericRepository(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddAsync(T entity)
        {
           await _dbcontext.AddAsync(entity);
        }

        public void DeleteAsync(T entity)
        {
            _dbcontext.Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T)==typeof(Product))
            {
                return (IReadOnlyList<T>)await _dbcontext.Set<Product>().Include(B=>B.Brand).Include(C=>C.Category).ToListAsync();
            }
            return await _dbcontext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec).ToListAsync();
        
        }

        public async Task<T> GetAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {
                return await _dbcontext.Set<Product>().Where(P => P.Id == id).Include(B => B.Brand).Include(C => C.Category).FirstOrDefaultAsync() as T;
            }
            return await _dbcontext.Set<T>().FindAsync(id);
            
        }

        public async Task<int> GetCountForPaginaion(ISpecification<T> spec)
        {
            return  SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec).Count();
        }

        public async Task<T> GetWithSpecAsync(ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>() , spec).FirstOrDefaultAsync();

        }

        public void UpdateAsync(T entity)
        {
            _dbcontext.Update(entity);
        }
    }
}
