using Application.DTOs.HealthCheck;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Service Health Check",
            Description = "Check the health of this service instance. The response should include the overall health of the service, as well as the result of each of the health check probes that were executed during the health check.",
            OperationId = "CheckHealth",
            Tags = new[] { "HealthCheck" }
        )]
        [SwaggerResponse(200, "Returns a HealthCheckResponse object that contains information about the overall health of the service and the result of each health check probe.", typeof(HealthCheckResponse))]
        [SwaggerResponse(503, "Returns a HealthCheckResponse object that contains information about the overall health of the service and the result of each health check probe.", typeof(HealthCheckResponse))]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();
            var result = new HealthCheckResponse
            {
                Status = report.Status.ToString(),
                Checks = report.Entries.Select(entry => new HealthCheckEntry
                {
                    Name = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Description = entry.Value.Description
                })
            };

            if (report.Status == HealthStatus.Healthy)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, result);
            }
        }
    }
}