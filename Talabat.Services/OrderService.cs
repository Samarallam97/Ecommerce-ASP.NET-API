
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.RepositoryInterfaces;
using Talabat.Core.ServiceInterfaces;
using Talabat.Core.Specifications.Orders;

namespace Talabat.Service
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;


		public OrderService(IBasketRepository basketRepository , IUnitOfWork unitOfWork)
        {
			_basketRepository=basketRepository;
			_unitOfWork=unitOfWork;
		}
		public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			// 1. Get basket from baskets Repo

			var basket =  _basketRepository.GetBasket(basketId);

			// 2. Get selected items at basket from products Repo

			var orderItems = new List<OrderItem>();
			
			if(basket?.Items?.Count > 0)
			{
				var orderRepository = _unitOfWork.Repository<Product>();

				foreach (var item in basket.Items)
				{
					var product =  orderRepository.GetById(item.ProductId);
					
					if(product is not null)
					{
						var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

						var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

						orderItems.Add(orderItem);
					}

				}
			}

			// 3. Calculate SubTotal

			var subTotal = orderItems.Sum(orderItem =>  orderItem.Price * orderItem.Quantity);
			
			// 4. Get DeliveryMethod from DeliveryMethods Repo

			var deliveryMethod =  _unitOfWork.Repository<DeliveryMethod>().GetById(deliveryMethodId);

			// 5. Create Order

			var order = new Order(buyerEmail , shippingAddress , deliveryMethod , orderItems , subTotal);

			 _unitOfWork.Repository<Order>().Add(order);

			// 6. Save to database [ need Unit of work]

			var result = await _unitOfWork.CompleteAsync();
			if (result == 0) return null;
			return order;
			
		}

		public IReadOnlyList<Order> GetOrdersForUser(string buyerEmail)
		{
			var orderRepository = _unitOfWork.Repository<Order>();

			var spec = new OrderSpecifications(buyerEmail);

			var orders = orderRepository.GetAllWithSpec(spec);

			return orders;
		}

		public Order? GetOrderByIdForUserAsync(int orderId, string buyerEmail)
		{
			var orderRepository = _unitOfWork.Repository<Order>();

			var spec = new OrderSpecifications(orderId, buyerEmail);

			var order = orderRepository.GetByIdWithSpec(spec);

			return order;
		}

		public IReadOnlyList<DeliveryMethod> GetDeliveryMethodsAsync()
		{
			var deliveryMethodRepository = _unitOfWork.Repository<DeliveryMethod>();

			var deliveryMethods =  deliveryMethodRepository.GetAll();

			return deliveryMethods;
		}

	}
}
