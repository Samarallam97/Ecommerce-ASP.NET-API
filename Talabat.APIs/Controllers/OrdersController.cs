using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.Order;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.ServiceInterfaces;

namespace Talabat.APIs.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrdersController(IOrderService orderService, IMapper mapper)
		{
			_orderService=orderService;
			_mapper=mapper;
		}

		[ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var address = _mapper.Map<AddressDTO, Address>(orderDTO.ShippingAddress);

			var order = await _orderService.CreateOrderAsync(buyerEmail, orderDTO.BasketId, orderDTO.DeliveryMethodId, address);

			if (order is null) return BadRequest(new BaseErrorResponse(400));

			return Ok(_mapper.Map<Order, OrderToReturnDTO>(order));
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetUserOrders()
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var orders =  _orderService.GetOrdersForUser(buyerEmail);

			return Ok(_mapper.Map<IReadOnlyList<Order> , IReadOnlyList<OrderToReturnDTO>>(orders));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<OrderToReturnDTO>> GetUserOrder(int id)
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var order =  _orderService.GetOrderByIdForUserAsync(id, buyerEmail);

			if (order is null)
				return NotFound(new BaseErrorResponse(404));

			return Ok(_mapper.Map<Order,OrderToReturnDTO>(order) );
		}


		[HttpGet("deliverymethods")] // static segment
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var deliveryMethods =  _orderService.GetDeliveryMethodsAsync();

			return Ok(deliveryMethods);
		}

	}
}
