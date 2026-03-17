using Noon.Core;
using Noon.Core.Entities;
using Noon.Core.Entities.OrderAggregate;
using Noon.Core.Entities.PruductModule;
using Noon.Core.Repositories;
using Noon.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable  _repositoris;
      
        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenaricRepository<TEntity>? Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositoris is  null) 
                _repositoris = new Hashtable();

            var type = typeof(TEntity).Name;

            if(!_repositoris.ContainsKey(type) )
            {

                var repository = new GenaricRepository<TEntity>(_dbContext);

                _repositoris.Add(type, repository);
            }
            return _repositoris[type] as IGenaricRepository<TEntity>; //explict casting

        }

        public async Task<int> Complete()
            => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
          => await _dbContext.DisposeAsync();

    }
}
