using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs.Basket;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket_Module;
using Talabat.Core.RepositoryInterfaces;

namespace Talabat.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BasketController : ControllerBase
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository , IMapper mapper)
		{
			_basketRepository=basketRepository;
			_mapper=mapper;
		}


		[HttpGet]

		public ActionResult<Basket> GetBasket(string id)
		{
			var basket = _basketRepository.GetBasket(id);
			if (basket is null)
				return new Basket(id);

			return Ok(basket);
		}

		[HttpPost]
		public ActionResult<Basket> CreateOrUpdateBasket(BasketDTO basket)
		{
			var goodBasket = _mapper.Map<Basket>(basket);

			var newBasket = _basketRepository.CreateOrUpdateBasket(goodBasket);
			if (newBasket is null)
				return BadRequest(new BaseErrorResponse(400));
			return Ok(newBasket);
		}

		[HttpDelete]
		public ActionResult<Basket> DeleteBasket(string id)
		{
			var basket = _basketRepository.DeleteBasket(id);
			if (basket is null)
				return NotFound(new BaseErrorResponse(404 , "this basket not exist"));
			return Ok(basket);
		}

	}
}
