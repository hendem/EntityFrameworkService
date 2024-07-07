using Infrastructure.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;

namespace HealthCheckTests
{
    public class HealthCheckTests
    {
        [Fact]
        public async Task HealthCheckTest()
        {
            
            var mockLogger = new Mock<ILogger<HealthCheck>>();

            var testHealthCheck = new HealthCheck(mockLogger.Object);
            var result = await testHealthCheck.CheckHealthAsync(new HealthCheckContext());

            Assert.Equal(HealthStatus.Healthy, result.Status);
        }
    }
}