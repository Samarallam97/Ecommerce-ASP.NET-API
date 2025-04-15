using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate next;
		private readonly IHostEnvironment env;
		private readonly ILogger<ExceptionHandlingMiddleware> logger;

		public ExceptionHandlingMiddleware(RequestDelegate next , IHostEnvironment env , ILogger<ExceptionHandlingMiddleware> logger)
        {
			this.next=next;
			this.env=env;
			this.logger=logger;
		}

		public async Task InvokeAsync(HttpContext HttpContext)
		{
			try
			{
				await next.Invoke(HttpContext);
			}
			catch (Exception ex)
			{
				/// Log The Exception
				logger.LogError(ex , ex.Message);

				/// HEADER
				HttpContext.Response.ContentType = "application/json";
				HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				
				/// BODY
				var response = env.IsDevelopment() ? new ExceptionResponse(500, ex.Message, ex.StackTrace)
													: new ExceptionResponse(500);

				var options = new JsonSerializerOptions()
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};

				var bodyAsString = JsonSerializer.Serialize(response);

				await HttpContext.Response.WriteAsync(bodyAsString);

			}
		}
    }
}
