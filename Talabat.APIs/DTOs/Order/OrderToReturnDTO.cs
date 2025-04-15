using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.DTOs.Order
{
	public class OrderToReturnDTO
	{
        public int Id { get; set; }
        public string BuyerEmail { get; set; }

		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

		public string Status { get; set; }

		public Address ShippingAddress { get; set; }

		public decimal Subtotal { get; set; }

		public decimal Total { get; set; }

		public string PaymentIntentId { get; set; } = "";

		public string DeliveryMethod { get; set; }
		public decimal DeliveryMethodCost { get; set; }

		public ICollection<OrderItemDTO> Items { get; set; } = new HashSet<OrderItemDTO>();
	}
}
