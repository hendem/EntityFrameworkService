using System.Collections.Generic;

namespace Application.DTOs.HealthCheck
{
    public class HealthCheckResponse
    {
        public string? Status { get; set; }
        public IEnumerable<HealthCheckEntry>? Checks { get; set; }
    }
}
