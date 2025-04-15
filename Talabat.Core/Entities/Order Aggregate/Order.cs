using System.ComponentModel.DataAnnotations.Schema;

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class Order : BaseEntity
	{
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subtotal)
		{
			BuyerEmail=buyerEmail;
			ShippingAddress=shippingAddress;
			DeliveryMethod=deliveryMethod;
			Items=items;
			Subtotal=subtotal;
		}

		public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address ShippingAddress { get; set; }

        public decimal Subtotal { get; set; }

        [NotMapped]
        public decimal Total => Subtotal + DeliveryMethod.Cost;
        
        public string PaymentIntentId { get; set; } = "";

		public int? DeliveryMethodId { get; set; }
		public DeliveryMethod DeliveryMethod { get; set; }

		public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
	}
}
