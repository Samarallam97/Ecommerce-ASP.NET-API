using Talabat.Core.Entities.Basket_Module;

namespace Talabat.APIs.DTOs.Basket
{
	public class BasketDTO
	{
		public string Id { get; set; }
		public List<BasketItemDTO> Items { get; set; }

		public int? DeliveryMethodId { get; set; }
		public decimal ShippingPrice { get; set; }

		public string? PaymentIntentId { get; set; }
		public string? ClientSecret { get; set; }
	}
}
