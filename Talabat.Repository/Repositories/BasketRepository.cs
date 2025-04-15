using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket_Module;
using Talabat.Core.RepositoryInterfaces;

namespace Talabat.Repository.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase redisDB;
        public BasketRepository(IConnectionMultiplexer redis)
        {
			redisDB = redis.GetDatabase();
		}

		public Basket? CreateOrUpdateBasket(Basket basket)
		{
			var created = redisDB.StringSet(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

			if (created)
				return basket;
			else
				return null;
		}

		public Basket? DeleteBasket(string id)
		{
			var basket = GetBasket(id);
			var deleted =  redisDB.KeyDelete(id);

			return deleted ? basket : null;

		}

		public Basket? GetBasket(string id)
		{
			var basket = redisDB.StringGet(id);
      
            return basket.IsNullOrEmpty? null : JsonSerializer.Deserialize<Basket>(basket);
		}

	
	}
}
