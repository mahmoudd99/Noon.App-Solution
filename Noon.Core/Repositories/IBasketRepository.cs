using Noon.Core.Entities.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Repositories
{
    public interface IBasketRepository
    {

        Task<CustomerBasket?> GetBasketAsync(string basketId);

        Task<CustomerBasket?> UpadateBasketAsync(CustomerBasket basket);

        Task<bool> DeleteBasketAsync(string basketId);


    }
}
