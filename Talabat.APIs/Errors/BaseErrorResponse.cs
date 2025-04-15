using System.Diagnostics;

namespace Talabat.APIs.Errors
{
	public class BaseErrorResponse
	{
		/// Use Cases
		/// NotFound()
		/// BadRequest()
		/// Unauthorized()
		
		public int StatusCode { get; set; }
		public string? Message { get; set; }

        public BaseErrorResponse(int statusCode , string message = null)
        {
			StatusCode = statusCode;
			Message = message ?? GetStatusCodeDefaultMessage(statusCode);
		}


		/////////////////////////////////////////////////////////
		private string GetStatusCodeDefaultMessage(int statusCode)
		{
			return statusCode switch
			{
				400 => "Bad Request",
				401 => "UnAuthorized",
				404 => "Resource Wasn't Found",
				500 => "Internal Server Error",
				_ => ""
			};
		}

	}
}
