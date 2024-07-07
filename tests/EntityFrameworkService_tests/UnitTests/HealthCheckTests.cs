using System.Threading.Tasks;
using EntityFrameworkService;
using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

namespace EntityFrameworkService_tests.UnitTests
{

    public class HealthCheckTests : IClassFixture<WebAppFactory<Startup>>
    {
        private readonly WebAppFactory<Startup> _factory;

        public HealthCheckTests(WebAppFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CheckHealthAsync_ReturnsHealthyStatus()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/health");

            var result = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Healthy", result);
            Assert.DoesNotContain("UnHealthy", result);
        }
        
    }
}