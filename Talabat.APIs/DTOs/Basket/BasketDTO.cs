using Talabat.Core.Entities.Basket_Module;

namespace Talabat.APIs.DTOs.Basket
{
	public class BasketDTO
	{
		public string Id { get; set; }
		public List<BasketItemDTO> Items { get; set; }
	}
}
