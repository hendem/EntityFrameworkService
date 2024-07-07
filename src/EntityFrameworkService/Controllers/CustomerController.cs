using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Models.DTOs.Customer;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;
using EntityFrameworkService.Validation;

namespace EntityFrameworkService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        // GET: api/<CustomerController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> Get([FromQuery] string partialName, [FromQuery] string partialPhoneNumber, [FromQuery] int pageNumber = 1, [FromQuery] int resultsPerPage = 10)
        {

            IEnumerable<Customer> customers = await _customerService.GetCustomersAsync(partialName, partialPhoneNumber, pageNumber, resultsPerPage);

            if (customers.Count() > 0)
            {
                return Ok(customers);
            }
            return NotFound(customers);
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(string id)
        {
            var customer = await _customerService.GetCustomerAsync(id);

            if (customer == null) return NotFound();

            return customer;
        }

        // POST api/<CustomerController>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<Customer>> Post([FromBody] Customer customer)
        {
            var createdCustomer = await _customerService.AddCustomerAsync(customer);
            return CreatedAtAction(nameof(Get), new { id = createdCustomer.CustomerId }, createdCustomer );
        }


        private ActionResult<Customer> BadRequest(Dictionary<string, List<string>> errors)
        {
            string traceId= Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            string X_GP_Request_Id = HttpContext.Request.Headers["X-GP-Request-Id"];
            var errorResponse = ValidationErrorResponse.GetValidationErrorResult(traceId, X_GP_Request_Id, errors);

            return BadRequest(errorResponse);
        }

        // PUT api/<CustomerController>
        [HttpPut]
        public async Task<ActionResult<Customer>> Put([FromBody] Customer customer)
        {            
            try
            {
                return Ok(await _customerService.UpdateCustomerAsync(customer));
            }
            catch(ArgumentException ex)
            {               
                return BadRequest(new Dictionary<string, List<string>> { { "CustomerId", new List<string> { ex.Message } } });
            }
        }

    }
}
