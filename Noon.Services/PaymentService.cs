using Microsoft.Extensions.Configuration;
using Noon.Core;
using Noon.Core.Entities.BasketModule;
using Noon.Core.Entities.OrderAggregate;
using Noon.Core.Entities.PruductModule;
using Noon.Core.Repositories;
using Noon.Core.Services;
using Noon.Core.Specifications;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Noon.Core.Entities.PruductModule.Product;

namespace Noon.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork
            , IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;

            _configuration = configuration;
        }
        public async Task<CustomerBasket> CreateeOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSetting:Secretkey"];


            //1. Create Basket
            var basket = await _basketRepository.GetBasketAsync(basketId);
            var shippingPrice = 0m;

            if (basket == null) return null;


            if (basket.DelivertMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DelivertMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
                basket.ShippingCost = deliveryMethod.Cost;
            }

            // 2.  Create ShippingPrice
            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {

                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }


            // 1+2 Create Amount

            PaymentIntent paymentIntent;
            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create PaymenyIntent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(I => I.Price * I.Quantity * 100) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else                                              // Upadte PaymenyIntent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(I => I.Price * I.Quantity * 100) + (long)shippingPrice * 100
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.UpadateBasketAsync(basket);
            return basket;

        }

        public async Task<Order> UpdatePaymentIntentToSucceededOrFailed(string pamentIntentId, bool isSucceeded)
        {
            var spec = new OrderWithPaymentIdSpec(pamentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            if (isSucceeded)
               order.Staus=OrderStaus.PaymentRecived;
            else
                order.Staus = OrderStaus.PaymentFailed;

             _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Complete();
            return order;
        }
    }
}
