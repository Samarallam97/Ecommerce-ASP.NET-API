using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket_Module;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.ServiceInterfaces
{
	public interface IPayementService
	{
		Task<Basket?> CreateOrUpdatePayementIntent(string basketId);
		Task<Order> HandlePaymentIntentSucceededOrFailed(string paymentIntentId, bool isSucceeded);
	}
}
