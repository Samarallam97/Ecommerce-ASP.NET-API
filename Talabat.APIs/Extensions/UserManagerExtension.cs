using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extensions
{
	public static class UserManagerExtension
	{
		public async static Task<AppUser> FindByEmailIncludingAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);

			var user = await userManager.Users.Include(u => u.Address).SingleOrDefaultAsync(u => u.Email == email);

			return user;
		}
	}
}
