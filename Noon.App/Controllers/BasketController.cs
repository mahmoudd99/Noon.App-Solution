using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noon.App.Dtos;
using Noon.App.Errors;
using Noon.Core.Entities.BasketModule;
using Noon.Core.Repositories;

namespace Noon.App.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketrepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketrepo ,IMapper mapper)
        {
            _basketrepo = basketrepo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]  //GET : api/basket/id
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketrepo.GetBasketAsync(id);
            return basket ?? new CustomerBasket(id);
        }

        [HttpPost]  //POST :/api/basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var maapbasket = _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
            var createdOrUpdatedBasket = await _basketrepo.UpadateBasketAsync(maapbasket);
            if (createdOrUpdatedBasket == null) return BadRequest(new  ApiResponse(400));
            return createdOrUpdatedBasket;

        }
        [HttpDelete] // DELETE : /a pi/basket
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
           return  await _basketrepo.DeleteBasketAsync(basketId);

        }


    }
}
