using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket_Module;

namespace Talabat.Core.RepositoryInterfaces
{
	public interface IBasketRepository
	{
		Basket? GetBasket(string id);
		Basket? CreateOrUpdateBasket(Basket basket);
		Basket? DeleteBasket(string id);
	}
}
