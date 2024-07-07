namespace Infrastructure.HealthChecks
{
    using System;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Logging;

    public class HealthCheck : IHealthCheck
    {

        private readonly ILogger<HealthCheck> _logger;

        public HealthCheck(ILogger<HealthCheck> logger)
        {
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                //TODO: Test something here.
                await Task.CompletedTask; //<-- delete me, just to make it async.

                _logger.LogDebug("Healh check is healthy.");
                return HealthCheckResult.Healthy();                
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(ex.Message);
            }
        }
    }

}
