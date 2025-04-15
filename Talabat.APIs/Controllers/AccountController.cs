using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.Account;
using Talabat.APIs.DTOs.Basket;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.ServiceInterfaces;

namespace Talabat.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IAuthService _authService;
		private readonly IMapper _mapper;

		public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager , IAuthService authService , IMapper mapper)
		{
			_userManager=userManager;
			_signInManager=signInManager;
			_authService=authService;
			_mapper=mapper;
		}

		[HttpPost("Register")]
		public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
		{
			if (CheckEmailExists(model.Email).Result.Value)
				return BadRequest(new ValidationErrorResponse()
				{
					Errors = new string[]
					{
						"This email is already in use"
					}
				});

			var user = new AppUser()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				UserName = model.Email.Split('@')[0]
			};
			var result = await _userManager.CreateAsync(user , model.Password);

			if (!result.Succeeded)
				return BadRequest(new BaseErrorResponse(400));

			return Ok(new UserDTO()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				Token = await _authService.CreateTokenAsync(user , _userManager)
			});
		}

		[HttpPost("Login")]
		public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
		{
			#region Email check
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is null)
				return Unauthorized(new BaseErrorResponse(401)); 
			#endregion

			#region Password check
			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

			if (!result.Succeeded)
				return Unauthorized(new BaseErrorResponse(401)); 
			#endregion

			return Ok(new UserDTO()
			{
				DisplayName = user.DisplayName,
				Email =user.Email,
				Token = await _authService.CreateTokenAsync(user, _userManager)
			});
		}

		[Authorize] 
		[HttpGet]
		public async Task<ActionResult<UserDTO>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await _userManager.FindByEmailAsync(email);

			return Ok(new UserDTO()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				//TODO:: Should be brought from DB
				Token = await _authService.CreateTokenAsync(user, _userManager)
			});
		}

		[Authorize] 
		[HttpGet("address")]
		public async Task<ActionResult<AddressDTO>> GetCurrentUserAddress()
		{

			var user = await _userManager.FindByEmailIncludingAddressAsync(User);

			var addressDTO = _mapper.Map<Address, AddressDTO>(user.Address);

			return Ok(addressDTO);
		}

		[Authorize]
		[HttpPut("address")]

		public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO updatedAddress)
		{
			var address = _mapper.Map<AddressDTO, Address>(updatedAddress);

			var user = await _userManager.FindByEmailIncludingAddressAsync(User);

			address.Id = user.Address.Id;
			user.Address = address;

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
				return BadRequest(new BaseErrorResponse(400));

			return Ok(updatedAddress);
		}

		[HttpGet("emailexists")]
		public async Task<ActionResult<bool>> CheckEmailExists(string email)
		{
			return await _userManager.FindByEmailAsync(email) is not null;
		}
	}
}
