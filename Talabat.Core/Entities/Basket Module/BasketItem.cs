using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Basket_Module
{
	public class BasketItem
	{

        public int ProductId { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
        public  string PictureUrl { get; set; }
        public string Category { get; set; }
		public string Brand { get; set; }
		public int Quantity { get; set; }
	}
}
