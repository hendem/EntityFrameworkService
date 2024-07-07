
using EntityFrameworkService.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkService_tests.UnitTests.Middleware
{
    public class CookieSignatureMiddlewareTest
    {
        [Fact]
        public async Task AppendSignatureToAuthorizationHeader()
        {
            // Arrange
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseMiddleware<CookieSignatureMiddleware>();
                    app.Run(async context =>
                    {
                        // Echo the Authorization header back in the response
                        var authorization = context.Request.Headers["Authorization"].ToString();
                        await context.Response.WriteAsync(authorization);
                    });
                });
            var server = new TestServer(builder);
            var client = server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/");
            request.Headers.Add("Authorization", "Bearer token");
            request.Headers.Add("Cookie", "acctoken=signature");

            // Act
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("Bearer token signature", content);
        }
    }
}