using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs.Basket
{
	public class BasketItemDTO
	{

		[Required]
		public int ProductId { get; set; }
		[Required]
		public string ProductName { get; set; }
		
		[Required]
		[Range(0.1, double.MaxValue, ErrorMessage = "Price should be greater than zero")]
		public decimal Price { get; set; }
		[Required]
		public string PictureUrl { get; set; }
		[Required]
		public string Category { get; set; }
		[Required]
		public string Brand { get; set; }
		[Required]

		[Range(1, int.MaxValue, ErrorMessage = "Quantity Can't be zero")]
		public int Quantity { get; set; }
	}
}

