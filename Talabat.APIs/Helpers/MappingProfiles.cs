using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.Basket;
using Talabat.APIs.DTOs.Order;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Basket_Module;
using Talabat.Core.Entities.Order_Aggregate;
using UserAddress = Talabat.Core.Entities.Identity.Address;

namespace Talabat.APIs.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles( )
		{
			CreateMap<Product, ProductDTO>().
				ForMember(p => p.BrandName,
				options => options.MapFrom(p => p.Brand.Name))
				.ForMember(p => p.CategoryName,
				options => options.MapFrom(p => p.Category.Name))
				.ForMember(p => p.PictureUrl,
				options => options.MapFrom<ProductPictureUrlResolver>());

			CreateMap<BasketDTO, Basket>();
			CreateMap<BasketItemDTO, BasketItem>();
			CreateMap<AddressDTO, Address>();

			CreateMap< UserAddress, AddressDTO>().ReverseMap();


			CreateMap<Order, OrderToReturnDTO>()
				.ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
				.ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

			CreateMap<OrderItem, OrderItemDTO>()
				.ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
				.ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
				.ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
				.ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());

		}

		#region MyRegion
		//public MappingProfiles(IConfiguration configuration)
		//{
		//    CreateMap<Product, ProductDTO>().
		//        ForMember(p => p.BrandName,
		//        options => options.MapFrom(p => p.Brand.Name))
		//        .ForMember(p => p.CategoryName ,
		//        options => options.MapFrom(p => p.Category.Name))
		//        .ForMember(p => p.PictureUrl ,
		//        options => options.MapFrom(p => $"{configuration["BaseUrl"]}/{p.PictureUrl}"));
		//} 
		#endregion
	}
}
