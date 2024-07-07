
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc;
using EntityFrameworkService;
using EntityFrameworkService.Controllers;

namespace EntityFrameworkService_tests.UnitTests
{
    public class WeatherControllerTests : IClassFixture<WebAppFactory<Startup>>
    {
        private readonly WebAppFactory<Startup> _factory;

        public WeatherControllerTests(WebAppFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_ReturnsSuccessResult()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/WeatherForcast");

            var result = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotNull(result);
        }
    }
}
