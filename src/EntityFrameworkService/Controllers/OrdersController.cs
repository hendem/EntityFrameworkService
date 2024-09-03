using Application.Models.DTOs.Order;
using Application.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EntityFrameworkService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _OrderService;

        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
        {
            _logger = logger;
            _OrderService = orderService;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetByCustomer([FromQuery] string customerId, [FromQuery] int pageNumber = 1, [FromQuery] int resultsPerPage = 10)
        {
            var orders = await _OrderService.GetOrdersByCustomerIdAsync(customerId, pageNumber, resultsPerPage);
            if(!orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("byEmployeeId")]
        public async Task<ActionResult<IEnumerable<Order>>> GetByEmployee([FromQuery] int employeeId, [FromQuery] int pageNumber = 1, [FromQuery] int resultsPerPage = 10)
        {
            var orders = await _OrderService.GetOrdersByEmployeeIdAsync(employeeId, pageNumber, resultsPerPage);
            if (!orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> Get(int orderId)
        {
            var order = await _OrderService.GetOrderAsync(orderId);
            if (order == null) return NotFound();
            return Ok(order);
        }

       
       
    }
}
