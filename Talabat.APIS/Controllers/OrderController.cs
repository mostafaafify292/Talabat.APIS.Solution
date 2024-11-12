using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService , IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(Order) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(orderDTO orderDTO)
        {
            var mappedAddress=_mapper.Map<AddressDTO , Address>(orderDTO.ShippingAddress);
            var order =await _orderService.CreateOrderAsync(orderDTO.BuyerEmail, orderDTO.BasketId
                                           , orderDTO.DeliveryMethodId, mappedAddress);
            if (order is null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Order, OrderToReturnDTO>(order));
            
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser(string Email)
        {
            var orders =await _orderService.GetOrdersForUserAsync(Email);
            return Ok(orders);
        }
        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var order =await _orderService.GetOrderByIdForUserAsync(orderId, buyerEmail);
            if (order is null) return NotFound(new ApiResponse(404));
            return Ok(order);
        }
    }
}
