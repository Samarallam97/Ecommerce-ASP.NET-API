using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Middlewares;
using Talabat.Repository.Data;
using Talabat.Repository.Data.DataSeeding;
using Talabat.Repository.Identity;


namespace Talabat.APIs
{
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Services Container

			builder.Services.AddControllers();

			#region Databases

			builder.Services.AddDbContext<StoreContext>(options =>
			{
				options/*.UseLazyLoadingProxies()*/
					   .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddDbContext<IdentityContext>(options =>
			{
				options/*.UseLazyLoadingProxies()*/
					   .UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});

			builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
			{
				var connectionString = builder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connectionString);
			});

			#endregion

			builder.Services.AddApplicationServices();

			builder.Services.AddIdentityServices(builder.Configuration);

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("MyPolicy", options =>
				{
					options.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]);
				});
			});

			builder.Services.AddSwaggerServices();

			#region Validation Error Handling

			builder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
														.SelectMany(p => p.Value.Errors)
														.Select(e => e.ErrorMessage)
														.ToArray();

					var validationErrorResponse = new ValidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(validationErrorResponse);
				};
			});
			#endregion

			#endregion

			var app = builder.Build();

			#region Update-Database

			var scope = app.Services.CreateScope();
			var serviceProvider = scope.ServiceProvider;

			var context = serviceProvider.GetService<StoreContext>();
			var _identityContext = serviceProvider.GetRequiredService<IdentityContext>();

			var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

			try
			{
				await context.Database.MigrateAsync();
				await _identityContext.Database.MigrateAsync();

			}
			catch (Exception ex) 
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "An error has occurred while updating the database");
			}
			#endregion

			#region DataSeeding

			DataSeeder.Seed(context);

			#endregion

			#region MiddleWares

			#region ExceptionHandling
			app.UseMiddleware<ExceptionHandlingMiddleware>(); 
			#endregion

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseStaticFiles();

			app.UseHttpsRedirection();

			#region Not Found EndPoint Handling
			app.UseStatusCodePagesWithReExecute("/error/{0}");
			#endregion

			app.UseCors("MyPolicy");

			app.MapControllers();

			app.UseAuthentication();

			app.UseAuthorization();

			#endregion

			app.Run();
		}
	}
}
