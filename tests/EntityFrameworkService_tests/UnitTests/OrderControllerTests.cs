using Application.Models.DTOs.Order;
using EntityFrameworkService;
using Microsoft.Extensions.DependencyInjection;
using NorthWinds.Persistence.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkService_tests.UnitTests
{
    public class OrderControllerTests : IClassFixture<WebAppFactory<Startup>>
    {

        private readonly WebAppFactory<Startup> _factory;

        public OrderControllerTests(WebAppFactory<Startup> webAppFactory)
        {
            _factory = webAppFactory;
        }

        private string customerVINET = "{ \"customerId\": \"VINET\",\r\n  \"companyName\": \"Vins et alcools Chevalier\",\r\n  \"contactName\": \"Paul Henriot\",\r\n  \"contactTitle\": \"Accounting Manager\",\r\n  \"address\": \"59 rue de l'Abbaye\",\r\n  \"city\": \"Reims\",\r\n  \"region\": null,\r\n  \"postalCode\": \"51100\",\r\n  \"country\": \"France\",\r\n  \"phone\": \"26.47.15.10\",\r\n  \"fax\": \"26.47.15.11\"\r\n}";

        private string ordersVINET = "[\r\n  {\r\n    \"orderId\": 10248,\r\n    \"customerId\": \"VINET\",\r\n    \"employeeId\": 5,\r\n    \"orderDate\": \"1996-07-04T00:00:00\",\r\n    \"requiredDate\": \"1996-08-01T00:00:00\",\r\n    \"shippedDate\": \"1996-07-16T00:00:00\",\r\n    \"shipVia\": 3,\r\n    \"freight\": 32.38,\r\n    \"shipName\": \"Vins et alcools Chevalier\",\r\n    \"shipAddress\": \"59 rue de l'Abbaye\",\r\n    \"shipCity\": \"Reims\",\r\n    \"shipRegion\": null,\r\n    \"shipPostalCode\": \"51100\",\r\n    \"shipCountry\": \"France\",\r\n    \"customer\": null,\r\n    \"orderDetails\": [],\r\n    \"shipViaNavigation\": null\r\n  },\r\n  {\r\n    \"orderId\": 10274,\r\n    \"customerId\": \"VINET\",\r\n    \"employeeId\": 6,\r\n    \"orderDate\": \"1996-08-06T00:00:00\",\r\n    \"requiredDate\": \"1996-09-03T00:00:00\",\r\n    \"shippedDate\": \"1996-08-16T00:00:00\",\r\n    \"shipVia\": 1,\r\n    \"freight\": 6.01,\r\n    \"shipName\": \"Vins et alcools Chevalier\",\r\n    \"shipAddress\": \"59 rue de l'Abbaye\",\r\n    \"shipCity\": \"Reims\",\r\n    \"shipRegion\": null,\r\n    \"shipPostalCode\": \"51100\",\r\n    \"shipCountry\": \"France\",\r\n    \"customer\": null,\r\n    \"orderDetails\": [],\r\n    \"shipViaNavigation\": null\r\n  },\r\n  {\r\n    \"orderId\": 10295,\r\n    \"customerId\": \"VINET\",\r\n    \"employeeId\": 2,\r\n    \"orderDate\": \"1996-09-02T00:00:00\",\r\n    \"requiredDate\": \"1996-09-30T00:00:00\",\r\n    \"shippedDate\": \"1996-09-10T00:00:00\",\r\n    \"shipVia\": 2,\r\n    \"freight\": 1.15,\r\n    \"shipName\": \"Vins et alcools Chevalier\",\r\n    \"shipAddress\": \"59 rue de l'Abbaye\",\r\n    \"shipCity\": \"Reims\",\r\n    \"shipRegion\": null,\r\n    \"shipPostalCode\": \"51100\",\r\n    \"shipCountry\": \"France\",\r\n    \"customer\": null,\r\n    \"orderDetails\": [],\r\n    \"shipViaNavigation\": null\r\n  },\r\n  {\r\n    \"orderId\": 10737,\r\n    \"customerId\": \"VINET\",\r\n    \"employeeId\": 2,\r\n    \"orderDate\": \"1997-11-11T00:00:00\",\r\n    \"requiredDate\": \"1997-12-09T00:00:00\",\r\n    \"shippedDate\": \"1997-11-18T00:00:00\",\r\n    \"shipVia\": 2,\r\n    \"freight\": 7.79,\r\n    \"shipName\": \"Vins et alcools Chevalier\",\r\n    \"shipAddress\": \"59 rue de l'Abbaye\",\r\n    \"shipCity\": \"Reims\",\r\n    \"shipRegion\": null,\r\n    \"shipPostalCode\": \"51100\",\r\n    \"shipCountry\": \"France\",\r\n    \"customer\": null,\r\n    \"orderDetails\": [],\r\n    \"shipViaNavigation\": null\r\n  },\r\n  {\r\n    \"orderId\": 10739,\r\n    \"customerId\": \"VINET\",\r\n    \"employeeId\": 3,\r\n    \"orderDate\": \"1997-11-12T00:00:00\",\r\n    \"requiredDate\": \"1997-12-10T00:00:00\",\r\n    \"shippedDate\": \"1997-11-17T00:00:00\",\r\n    \"shipVia\": 3,\r\n    \"freight\": 11.08,\r\n    \"shipName\": \"Vins et alcools Chevalier\",\r\n    \"shipAddress\": \"59 rue de l'Abbaye\",\r\n    \"shipCity\": \"Reims\",\r\n    \"shipRegion\": null,\r\n    \"shipPostalCode\": \"51100\",\r\n    \"shipCountry\": \"France\",\r\n    \"customer\": null,\r\n    \"orderDetails\": [],\r\n    \"shipViaNavigation\": null\r\n  }\r\n]";

        private void SeedData(NorthWindsContext context)
        {
            if (!context.Customers.Any(x => x.CustomerId == "VINET"))
            {
                var customer = JsonSerializer.Deserialize<NorthWinds.Persistence.Entities.Customer>(customerVINET, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
                context.Customers.Add(customer);

                var employee1 = new NorthWinds.Persistence.Entities.Employee { EmployeeId = 1, FirstName = "Milton", LastName = "Nobody", Address = "123 fake st", City = "Fake town", Title = "Red Sapler" };
                var employee2 = new NorthWinds.Persistence.Entities.Employee { EmployeeId = 2, FirstName = "Worker", LastName = "Nobody", Address = "123 fake st", City = "Fake town", Title = "Do-er of things" };
                var employee3 = new NorthWinds.Persistence.Entities.Employee { EmployeeId = 3, FirstName = "Joe", LastName = "Nobody", Address = "123 fake st", City = "Fake town", Title = "Worker" };
                var employee4 = new NorthWinds.Persistence.Entities.Employee { EmployeeId = 5, FirstName = "Fred", LastName = "Nobody", Address = "123 fake st", City = "Fake town", Title = "Manager" };
                var employee5 = new NorthWinds.Persistence.Entities.Employee { EmployeeId = 6, FirstName = "John", LastName = "Nobody", Address = "123 fake st", City = "Fake town", Title = "Programmer" };
                context.Employees.Add(employee1);
                context.Employees.Add(employee2);
                context.Employees.Add(employee3);
                context.Employees.Add(employee4);
                context.Employees.Add(employee5);

                var shipper1 = new NorthWinds.Persistence.Entities.Shipper { ShipperId = 1, CompanyName = "Speedy Express", Phone = "(503) 555 - 9931" };
                var shipper2 = new NorthWinds.Persistence.Entities.Shipper { ShipperId = 2, CompanyName = "United Package", Phone = "(503) 555 - 9931" };
                var shipper3 = new NorthWinds.Persistence.Entities.Shipper { ShipperId = 3, CompanyName = "Federal Shipping", Phone = "(503) 555 - 9931" };
                context.Shippers.Add(shipper1);
                context.Shippers.Add(shipper2);
                context.Shippers.Add(shipper3);

                if (!context.Orders.Any(x => x.OrderId == 10248))
                {
                    var orders = JsonSerializer.Deserialize<List<NorthWinds.Persistence.Entities.Order>>(ordersVINET, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;                  
                    context.Orders.AddRange(orders);
                }
                context.SaveChanges();
            }
        }

        [Theory]
        [InlineData(10248)]
        [InlineData(10739)]
        [InlineData(10274)]
        public async Task  Get_Orders_By_OrderId_Test(int orderId)
        {
            //arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<NorthWindsContext>();
                SeedData(dbContext);
            }

            var client = _factory.CreateClient();            

            //act

            var response = await client.GetAsync($"/api/orders/{orderId}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var order = JsonSerializer.Deserialize<Order>(responseContent);

            //assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(orderId, order?.OrderId);

        }

        [Theory]        
        [InlineData(5)]
        [InlineData(2)]
        [InlineData(6)]
        public async Task Get_Orders_By_EmployeeId_Test(int employeeId)
        {
            //arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<NorthWindsContext>();
                SeedData(dbContext);
            }

            var client = _factory.CreateClient();

            //act

            var response = await client.GetAsync($"api/Orders/byEmployeeId?employeeId={employeeId}&pageNumber=1&resultsPerPage=10");

            var responseContent = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(responseContent)!;

            //assert
            Assert.NotNull(orders);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.All(orders, order => Assert.Equal(employeeId, order.EmployeeId));
        }



    }
}
