using Microsoft.AspNetCore.Mvc;
using Noon.Core;
using Noon.Core.Entities.OrderAggregate;
using Noon.Core.Entities.PruductModule;
using Noon.Core.Repositories;
using Noon.Core.Services;
using Noon.Core.Specifications;
using Noon.Repository.Spec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unit;

        //private readonly IGenaricRepository<Product> _genaricRepo;
        //private readonly IGenaricRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenaricRepository<Order> _orderRepo;

        public OrderService(
            IBasketRepository basketRep,
           IPaymentService paymentService
            ,IUnitOfWork unit)
        //IGenaricRepository<Product> genaricRepo,
        //IGenaricRepository<DeliveryMethod> deliveryMethodRepo,
        //IGenaricRepository<Order> orderRepo)
        {
            _basketRepo = basketRep;
            _paymentService = paymentService;
            _unit = unit;
            //_genaricRepo = genaricRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
            //_orderRepo = orderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippaddress)
        {
            /// 1. Get Basket From BasketRepo
            /// 2. Get Sekected Items at Basket From ProductRepo
            /// 3. Calculate Subtotal
            /// 4. Get DeliveryMethod from DeliveryMethod Repo
            /// 5. Create Order
            /// 6. save to DataBase

            var basket = await _basketRepo.GetBasketAsync(basketId); // 1.
            var OrderItems = new List<OrderItem>();// 2.

            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var productRepo = _unit.Repository<Product>();
                    if (productRepo != null)
                    {
                        var product = await productRepo.GetByIdAsync(item.Id);
                        var productOrderItem = new ProductItemOredred(product.Id, product.Name, product.PictureUrl);
                        var orderItem = new OrderItem(productOrderItem, product.Price, item.Quantity);
                        OrderItems.Add(orderItem);
                    }
                }

            }

            var subTotal = OrderItems.Sum(item => item.Price * item.Quantity);//3.

            DeliveryMethod deliveryMethod = new DeliveryMethod();// 4.

            var deliveryRepo = _unit.Repository<DeliveryMethod>();
            if (deliveryRepo != null)
                deliveryMethod = await deliveryRepo.GetByIdAsync(deliveryMethodId);

            var spec = new OrderWithPaymentIdSpec(basket.PaymentIntentId);
            var existingOrder = await _unit.Repository<Order>().GetByIdWithSpecAsync(spec);

            if (existingOrder != null)
            {
                _unit.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateeOrUpdatePaymentIntent(basketId);
            }

            var CreatedOrder = new Order(buyerEmail, shippaddress, deliveryMethod, OrderItems, subTotal,basket.PaymentIntentId);//5.

            var orderRepo = _unit.Repository<Order>();
            if (orderRepo != null)
            {
                await orderRepo.Add(CreatedOrder);
                var result = await _unit.Complete();
                if (result > 0)
                    return CreatedOrder;
            }

            return null;
        }


        public async Task<IReadOnlyList<Order?>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new orderSpecification(buyerEmail);
            var orderRepo =  _unit.Repository<Order>();
            var orders = await orderRepo.GetAllWithSpecAsync(spec);
            return orders;
        }

        Task<Order?> IOrderService.GetOrderForUserAsync(string buyerEmail, int orderId)
        {
            var spec = new orderSpecification(buyerEmail, orderId);
            var orderRepos = _unit.Repository<Order>();
            var order = orderRepos.GetByIdWithSpecAsync(spec);
            return order;
        }
        public async Task<DeliveryMethod> GetDeliveryMethodAsync(int deliveryMethodId)
        {
            var spec = new DeliveryMethodSpecification(deliveryMethodId);
            var deliveryRepo = _unit.Repository<DeliveryMethod>();
            var delivery = await deliveryRepo.GetByIdWithSpecAsync(spec);
            return delivery;
           

        }
    }
}
