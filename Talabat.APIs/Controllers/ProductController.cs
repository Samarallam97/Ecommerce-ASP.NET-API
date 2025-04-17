using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.RepositoryInterfaces;
using Talabat.Core.ServiceInterfaces;
using Talabat.Core.Specifications.Products;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;
		private readonly IMapper _mapper;

		public ProductsController(IProductService productService, IMapper mapper)
		{
			_productService=productService;
			_mapper=mapper;
		}

		[HttpGet]
		public ActionResult<IReadOnlyList<ProductDTO>> GetAll([FromQuery] ProductParams productParams)
		{
			var products = _productService.GetAllProducts(productParams);

			var ProductDTOs = _mapper.Map<IReadOnlyList<Product> , IReadOnlyList<ProductDTO>>(products);	

			if(productParams.PageSize is null )
				return Ok(ProductDTOs);
			else
			{
				var count = _productService.GetCount(productParams);

				PaginationResponse<ProductDTO> paginationResponse = new
					(productParams.PageSize.Value, productParams.PageIndex.Value, count, ProductDTOs);

				return Ok(paginationResponse);
			}
		}

		[HttpGet("{id}")]
		public ActionResult<ProductDTO> GetById(int id)
		{
			var result = _productService.GetProductById(id);

			if (result is null)
				return NotFound(new BaseErrorResponse(404));

			var ProductDTO = _mapper.Map<Product ,ProductDTO>(result);

			return Ok(ProductDTO);
		}

		[HttpGet("brands")]
		public ActionResult<IReadOnlyList<Brand>> GetBrands()
		{
			var brands = _productService.GetAllBrands();
			return Ok(brands);
		}

		[HttpGet("categories")]
		public ActionResult<IReadOnlyList<Category>> GetCategories()
		{
			var categories = _productService.GetAllCategories();
			return Ok(categories);
		}

	}
}
