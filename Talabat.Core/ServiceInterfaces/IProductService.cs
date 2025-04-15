using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.Products;

namespace Talabat.Core.ServiceInterfaces
{
	public interface IProductService
	{
		IReadOnlyList<Product> GetAllProducts(ProductParams specParams);
		Product? GetProductById(int id);
		int GetCount(ProductParams specParams);

		IReadOnlyList<Brand> GetAllBrands();
		IReadOnlyList<Category> GetAllCategories();
	}
}
