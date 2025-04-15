
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.ServiceInterfaces
{
	public interface IOrderService
	{
		Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress);
		IReadOnlyList<Order> GetOrdersForUser(string buyerEmail);
		Order? GetOrderByIdForUserAsync(int orderId, string buyerEmail);
		IReadOnlyList<DeliveryMethod> GetDeliveryMethodsAsync();

	}
}
