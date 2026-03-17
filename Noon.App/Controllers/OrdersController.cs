using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noon.App.Dtos;
using Noon.App.Errors;
using Noon.Core.Entities.OrderAggregate;
using Noon.Core.Services;
using System.Security.Claims;

namespace Noon.App.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(
            
            IOrderService orderService,
            IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]  // POST: /api/Orders
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var shippingAddress = _mapper.Map<AddressDto,Address>(orderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(email, orderDto.BasketId, orderDto.DeliveryMethodId,shippingAddress);
            if (order == null) return BadRequest();
             
            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GatOrdersForUser()
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if(userEmail == null) return BadRequest();

            var order = await _orderService.GetOrdersForUserAsync(userEmail);

            return Ok(_mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>(order));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderForUserAsync(userEmail, id);
            if (order is null) return NotFound();
            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }


        [HttpGet("delivery/{id}")]
        public async Task<ActionResult<DeliveryMethod>> GetDliveryMethod(int deliveryMethodId =1)
        {

            var delievery = await _orderService.GetDeliveryMethodAsync(deliveryMethodId);
            if(delievery is null) return NotFound();
            return Ok(delievery);

        }




    }
}
