﻿using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.DTOs
{
	public class OrderDTO
	{
        [Required]
        public string BasketId { get; set; }
		[Required]
		public int DeliveryMethodId { get; set; }
        public AddressDTO ShippingAddress { get; set; }
    }
}
