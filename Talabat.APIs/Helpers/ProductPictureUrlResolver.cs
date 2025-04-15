using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
	public class ProductPictureUrlResolver : IValueResolver<Product, ProductDTO, string>
	{
		private readonly IConfiguration _configuration;

		public ProductPictureUrlResolver(IConfiguration configuration)
        {
			_configuration=configuration;
		}
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
		{
			if(source.PictureUrl is not null)
				return $"{_configuration["BaseUrl"]}/{source.PictureUrl}";
			return "";
		}
	}
}
