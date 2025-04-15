using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Orders
{
	public class OrderSpecifications : Specification<Order>
	{
        public OrderSpecifications(string buyerEmail)
            :base(o => o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

			OrderByDesc = o => o.OrderDate;
		}

		public OrderSpecifications(int orderId , string buyerEmail)
			: base(o => o.Id == orderId && o.BuyerEmail == buyerEmail)
		{
			Includes.Add(o => o.DeliveryMethod);
			Includes.Add(o => o.Items);

		}


	}
}
