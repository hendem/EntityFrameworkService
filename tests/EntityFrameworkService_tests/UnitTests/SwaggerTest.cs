using EntityFrameworkService;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkService_tests.UnitTests
{
    public class SwaggerTest : IClassFixture<WebAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public SwaggerTest(WebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task SwaggerJsonIsAvailable()
        {
            var response = await _client.GetAsync("/swagger/v1/swagger.json");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("\"openapi\": \"3.0.1\"", content);
        }

        [Fact]
        public async Task SwaggerUIIsAvailable()
        {
            var response = await _client.GetAsync("/swagger/index.html");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Swagger UI", content);
        }
    }
}