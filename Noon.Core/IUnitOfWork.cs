using Noon.Core.Entities;
using Noon.Core.Entities.OrderAggregate;
using Noon.Core.Entities.PruductModule;
using Noon.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core
{
    public interface IUnitOfWork : IAsyncDisposable  
    {



        IGenaricRepository<TEntity>? Repository<TEntity>() where TEntity : BaseEntity;
        public Task<int> Complete();


       
    }
}
