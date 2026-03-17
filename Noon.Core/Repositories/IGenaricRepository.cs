using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noon.Core.Entities;
using Noon.Core.Specifications;

namespace Noon.Core.Repositories
{
    public interface IGenaricRepository<T> where T : BaseEntity
    {

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

        Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);
        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);


        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
