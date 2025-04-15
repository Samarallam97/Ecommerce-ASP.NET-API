using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs.Basket
{
	public class RegisterDTO
	{
		[Required]
        public string DisplayName { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{7,}$", ErrorMessage = "Password must be at least 7 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character (@$!%*?&).")]
		public string Password { get; set; }

	}
}
