using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration , IBasketRepository basketRepository , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            //Secret Key
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];
            //Get Basket
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;

            //Get DeliveryMethod
            var shipingPrice = 0M;   //Decimal
            if (basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
                shipingPrice = DeliveryMethod.Cost;
            }

            //Total = supTotal + Delivery.cost
            if (basket.items.Count > 0)
            {
                foreach (var item in basket.items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }
            var subTotal = basket.items.Sum(item => item.Price * item.Quantity);

            //Create Payment Intent

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId)) //Create
            {
                var optionsForCreate = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subTotal*100) + (long)(shipingPrice*100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() {"card"}
                };
                paymentIntent =await service.CreateAsync(optionsForCreate);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else  // Update
            {
                var optionsForUpdate = new PaymentIntentUpdateOptions()
                {
                    Amount  = (long)(subTotal * 100) + (long)(shipingPrice * 100)
                   
                };
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, optionsForUpdate);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            //update basket on Database
            await _basketRepository.UpdateBasketAsync(basket);
            return (basket);
        }

    }
}
