using System.Collections.Generic;

namespace Application.Models.DTOs.HealthCheck
{
    public class HealthCheckResponse
    {
        public string? Status { get; set; }
        public IEnumerable<HealthCheckEntry>? Checks { get; set; }
    }
}
