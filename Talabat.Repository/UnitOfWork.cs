using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbcontext;
        //private Dictionary<string, IGenericRepository<BaseEntity>> _repositories;
        private Hashtable _repository   ;
        public UnitOfWork(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
            //_repositories = new Dictionary<string, IGenericRepository<BaseEntity>>();
            _repository = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
             return await _dbcontext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
             await _dbcontext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;
            if (!_repository.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_dbcontext) ;
                _repository.Add(key, repository);
            }
            return _repository[key]as IGenericRepository<TEntity>;
        }
    }
}
