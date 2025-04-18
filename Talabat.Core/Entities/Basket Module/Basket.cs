﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Basket_Module
{
	public class Basket
	{
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }

        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }

        public string? PaymentIntentId { get; set; }
		public string? ClientSecret { get; set; }
		public Basket(string id)
        {
            Id = id;
            Items = new List<BasketItem>();
        }
    }
}
