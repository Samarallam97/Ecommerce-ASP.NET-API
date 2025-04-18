using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.RepositoryInterfaces;
using Talabat.Core.ServiceInterfaces;
using Talabat.Repository;
using Talabat.Repository.Identity;
using Talabat.Repository.Repositories;
using Talabat.Service;
using Talabat.Services;

namespace Talabat.APIs.Extensions
{
	public static class DependencyInjectionServices
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
		{

			#region Generic Repository

			//Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
			Services.AddScoped<IBasketRepository, BasketRepository>();
			Services.AddScoped(typeof(IOrderService), typeof(OrderService));
			Services.AddScoped(typeof(IProductService), typeof(ProductService));
			Services.AddScoped(typeof(IPayementService), typeof(PayementService));
			Services.AddSingleton(typeof(ICacheService), typeof(CacheService));

			#endregion

			#region Mapper

			Services.AddScoped<ProductPictureUrlResolver>();
			Services.AddScoped<OrderItemPictureUrlResolver>();
			Services.AddAutoMapper(m => m.AddProfile(typeof(MappingProfiles)));

			#endregion


			return Services;
		}

		public static IServiceCollection AddIdentityServices(this IServiceCollection Services , IConfiguration configuration)
		{

			Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

			Services.AddScoped(typeof(IAuthService) , typeof(AuthService));

			Services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

				}).AddJwtBearer(options =>
					{
						options.TokenValidationParameters = new TokenValidationParameters()
						{
							ValidateAudience = true,
							ValidAudience = configuration["JWT:Audience"],
							ValidateIssuer = true,
							ValidIssuer = configuration["JWT:Issuer"],
							ValidateIssuerSigningKey = true,
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
							ValidateLifetime = true,
							ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:ExpireInDays"]))
						};
					});

			return Services;
		}

		public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
		{
			Services.AddEndpointsApiExplorer();
			Services.AddSwaggerGen();

			return Services;
		}
	}
}
