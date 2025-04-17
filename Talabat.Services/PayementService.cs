using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities.Basket_Module;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.RepositoryInterfaces;
using Talabat.Core.ServiceInterfaces;
using Talabat.Core.Specifications.Orders;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Services
{
	public class PayementService : IPayementService
	{
		private readonly IConfiguration _configuration;
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;

		public PayementService(IConfiguration configuration , IBasketRepository basketRepository , IUnitOfWork unitOfWork )
        { 
			_configuration=configuration;
			_basketRepository=basketRepository;
			_unitOfWork=unitOfWork;
		}
        public async Task<Basket?> CreateOrUpdatePayementIntent(string basketId)
		{
			StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
			
			var basket = _basketRepository.GetBasket(basketId);

			if (basket is null) return null;

			var shippingPrice = 0m;

			if (basket.DeliveryMethodId.HasValue)
			{
				var deliveryMethod  = _unitOfWork.Repository<DeliveryMethod>().GetById(basket.DeliveryMethodId.Value);
				basket.ShippingPrice = deliveryMethod.Cost;

				shippingPrice = deliveryMethod.Cost;
			}

			if(basket.Items.Count > 0)
			{
				foreach (var item in basket.Items)
				{
					var product = _unitOfWork.Repository<Product>().GetById(item.ProductId);

					if(item.Price != product.Price)
						item.Price = product.Price;
				}
			}

			PaymentIntentService paymentIntentService = new PaymentIntentService();

			PaymentIntent paymentIntent;

			if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create Intent
			{
				var createOptions = new PaymentIntentCreateOptions()
				{
					Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100 ,
					Currency = "usd",
					PaymentMethodTypes = new List<string>() { "card" }
				};

				paymentIntent = await paymentIntentService.CreateAsync(createOptions); // Integrate with Stripe

				basket.PaymentIntentId = paymentIntent.Id;
				basket.ClientSecret = paymentIntent.ClientSecret;
			} 
			else // Update Intent
			{
				var updateOptions = new PaymentIntentUpdateOptions()
				{
					Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100
				};

				await paymentIntentService.UpdateAsync(basket.PaymentIntentId, updateOptions);
			}

			  _basketRepository.CreateOrUpdateBasket(basket);

            return basket;

        }

		public async Task<Order> HandlePaymentIntentSucceededOrFailed(string paymentIntentId , bool isSucceeded)
		{
			var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);

			var order = _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

			if(isSucceeded)
				order.Status = OrderStatus.PaymentRecieved;
			else
				order.Status = OrderStatus.PaymentFailed;

			_unitOfWork.Repository<Order>().Update(order);
			
			await _unitOfWork.CompleteAsync();

			return order;
		}
	}
}
