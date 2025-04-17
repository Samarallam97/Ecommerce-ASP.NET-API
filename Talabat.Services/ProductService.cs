using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.ServiceInterfaces;
using Talabat.Core.Specifications.Products;

namespace Talabat.Services
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(IUnitOfWork unitOfWork)
        {
			_unitOfWork=unitOfWork;
		}
        public IReadOnlyList<Product> GetAllProducts(ProductParams specParams)
		{
			var spec = new ProductIncludingBrandsAndCategory(specParams);

			var products =  _unitOfWork.Repository<Product>().GetAllWithSpec(spec);

			return products;
		}

		public  int GetCount(ProductParams specParams)
		{
			var countSpec = new ProductsWithFilterationForCountSpec(specParams);

			var count =  _unitOfWork.Repository<Product>().GetAll().Count();

			return count;
		}


		public Product? GetProductById(int id)
		{
			var spec = new ProductIncludingBrandsAndCategory(p => p.Id == id);

			var product =  _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

			return product;
		}
		public IReadOnlyList<Brand> GetAllBrands()
		{
			return  _unitOfWork.Repository<Brand>().GetAll();
		}

		public IReadOnlyList<Category> GetAllCategories()
		{
			return _unitOfWork.Repository<Category>().GetAll();
		}


	}
}
