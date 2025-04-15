using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.DataSeeding
{
    public static class DataSeeder
    {
        public static void Seed(StoreContext _context)
        {
            if (_context.Brands?.Count() == 0)
            {
                var brandsAsString = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/Files/brands.json");
                var brands = JsonSerializer.Deserialize<List<Brand>>(brandsAsString);

                foreach (var brand in brands)
                {
                    _context.Brands.Add(brand);
                }
            }

            if (_context.Categories?.Count() == 0)
            {
                var categoriesAsString = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/Files/categories.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesAsString);

                foreach (var category in categories)
                {
                    _context.Categories.Add(category);
                }
            }

            if (_context.Products?.Count() == 0)
            {
                var productsAsString = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/Files/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsAsString);

                foreach (var product in products)
                {
                    _context.Products.Add(product);
                }
            }

			if (_context.DeliveryMethods.Count() == 0)
			{
				var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/Files/delivery.json");

				var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

				if (deliveryMethods?.Count > 0)
				{
					foreach (var deliveryMethod in deliveryMethods)
					{
						_context.Set<DeliveryMethod>().Add(deliveryMethod);
					}
				}

			}


			_context.SaveChanges();
        }
    }
}
