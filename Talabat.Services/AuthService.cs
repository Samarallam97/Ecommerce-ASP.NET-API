using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.ServiceInterfaces;

namespace Talabat.Services
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _configuration;

		public AuthService(IConfiguration configuration)
		{
			_configuration=configuration;
		}
		public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
		{

			var privateClaims = new List<Claim>()
			{
				new Claim(ClaimTypes.GivenName , user.UserName),
				new Claim(ClaimTypes.Email , user.Email),
			};

			var userRoles = await userManager.GetRolesAsync(user);

			foreach (var role in userRoles)
			{
				privateClaims.Add(new Claim(ClaimTypes.Role, role));
			}

			var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

			var token = new JwtSecurityToken
			(
				audience: _configuration["JWT:Audience"],
				issuer: _configuration["JWT:Issuer"],
				expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:ExpireInDays"])),
				claims: privateClaims,
				signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
