using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpecifications;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository ,IUnitOfWork unitOfWork , IPaymentService paymentService)          
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1. Get Basket From basket repo

            var basket =await _basketRepository.GetBasketAsync(basketId);

            //2. Get Selected Items at Basket from product repo

            var orderItems = new List<OrderItem>();
            if (basket?.items.Count()>0)
            {
                foreach (var item in basket.items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }
            //3. Calculate Suptotal

            var supTotal = orderItems.Sum(orderitem=>orderitem.Price * orderitem.Quantity);

            //4. Get DeliveryMethod from DeliveryMethod repo

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

            //Additional if any order has the same PaymentIntentId
            var spec = new OrderWithPaymentIntentSpec(basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            if (ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().DeleteAsync(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }
            //5. Create Order

            var order = new Order(buyerEmail , shippingAddress, deliveryMethod , orderItems , supTotal, basket.PaymentIntentId  );

            await _unitOfWork.Repository<Order>().AddAsync(order);

            //6. Save To DataBase

            var result =await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;
            
        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpec(orderId, buyerEmail);
            var order = await orderRepo.GetWithSpecAsync(spec);
            return order;

        }
         
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync( string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpec (buyerEmail);
            var orders =await orderRepo.GetAllWithSpecAsync(spec);
            return orders;
        }
    }
}
