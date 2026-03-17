using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Noon.Core.Entities;
using Noon.Core.Entities.PruductModule;
using Noon.Core.Repositories;
using Noon.Core.Specifications;
using Noon.Repository.Data;
using Noon.Repository.Spec;

namespace Noon.Repository
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenaricRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();

        ///if (typeof(T) == typeof(Product))
        ///    return (IEnumerable<T>)await _dbContext.OderBy(P => P.Price).Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();
        ///else

        public async Task<T> GetByIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec) 
            =>  await ApplySpecification(spec).ToListAsync();
        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            
            return await ApplySpecification(spec).CountAsync();
        }

        private  IQueryable<T> ApplySpecification(ISpecification<T> spec)
        { 
          return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task Add(T entity) 
            => await _dbContext.Set<T>().AddAsync(entity);

        public void Update(T entity)
             => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
             => _dbContext.Set<T>().Remove(entity);
    }
}
