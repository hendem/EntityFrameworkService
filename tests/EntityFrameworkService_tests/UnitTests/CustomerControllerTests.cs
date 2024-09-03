using Application.Models.DTOs.Customer;
using EntityFrameworkService;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthWinds.Persistence.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkService_tests.UnitTests
{
    public class CustomerControllerTests : IClassFixture<WebAppFactory<Startup>>
    {

        private readonly WebAppFactory<Startup> _factory;


        public CustomerControllerTests(WebAppFactory<Startup> factory)
        {
            _factory = factory;
        }

        private void SeedData(NorthWindsContext dbContext)
        {
            // Add test data here.

            //This guard check is required because we are sharing the context between unit tests.
            if (!dbContext.Customers.Any(x => x.CustomerId == "ALFKI"))
            {
                dbContext.Customers.Add(new NorthWinds.Persistence.Entities.Customer { CustomerId = "ALFKI", CompanyName = "Alfreds Futterkiste", ContactName = "Maria Anders", ContactTitle = "Sales Representative", Address = "Obere Str. 57", City = "Berlin", Region = null, PostalCode = "12209", Country = "Germany", Phone = "030-0074321", Fax = "030-0076545" });
            }
            if (!dbContext.Customers.Any(x => x.CustomerId == "CHOPS"))
            {
                dbContext.Customers.Add(new NorthWinds.Persistence.Entities.Customer { CustomerId = "CHOPS", CompanyName = "Chop-suey Chinese", ContactName = "Yang Wang", ContactTitle = "Owner", Address = "Hauptstr. 29", City = "Bern", Region = null, PostalCode = "3012", Country = "Switzerland", Phone = "0452-076545", Fax = null });
            }
            dbContext.SaveChanges();
        }

        [Theory]
        [InlineData("ALFKI", "{\"customerId\":\"ALFKI\",\"companyName\":\"Alfreds Futterkiste\",\"contactName\":\"Maria Anders\",\"contactTitle\":\"Sales Representative\",\"address\":\"Obere Str. 57\",\"city\":\"Berlin\",\"region\":null,\"postalCode\":\"12209\",\"country\":\"Germany\",\"phone\":\"030-0074321\",\"fax\":\"030-0076545\"}")]
        [InlineData("CHOPS", "{\"customerId\":\"CHOPS\",\"companyName\":\"Chop-suey Chinese\",\"contactName\":\"Yang Wang\",\"contactTitle\":\"Owner\",\"address\":\"Hauptstr. 29\",\"city\":\"Bern\",\"region\":null,\"postalCode\":\"3012\",\"country\":\"Switzerland\",\"phone\":\"0452-076545\",\"fax\":null}")]
        public async Task Get_CustomerbyCustomerID_Test(string customerId, string exspectedResponse)
        {
            //Arrange

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<NorthWindsContext>();
                SeedData(dbContext);
            }

            var client = _factory.CreateClient();

            //Act

            var response = await client.GetAsync($"/api/customer/{customerId}");

            var result = await response.Content.ReadAsStringAsync();
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(exspectedResponse, result);

        }

        private const string jsonData = "[\r\n  {\r\n    \"customerId\": \"ROMEY\",\r\n    \"companyName\": \"Romero y tomillo\",\r\n    \"contactName\": \"Alejandra Camino\",\r\n    \"contactTitle\": \"Accounting Manager\",\r\n    \"address\": \"Gran Vía, 1\",\r\n    \"city\": \"Madrid\",\r\n    \"region\": null,\r\n    \"postalCode\": \"28001\",\r\n    \"country\": \"Spain\",\r\n    \"phone\": \"(91) 745 6200\",\r\n    \"fax\": \"(91) 745 6210\"\r\n  },\r\n  {\r\n    \"customerId\": \"TRADH\",\r\n    \"companyName\": \"Tradição Hipermercados\",\r\n    \"contactName\": \"Anabela Domingues\",\r\n    \"contactTitle\": \"Sales Representative\",\r\n    \"address\": \"Av. Inês de Castro, 414\",\r\n    \"city\": \"Sao Paulo\",\r\n    \"region\": \"SP\",\r\n    \"postalCode\": \"05634-030\",\r\n    \"country\": \"Brazil\",\r\n    \"phone\": \"(11) 555-2167\",\r\n    \"fax\": \"(11) 555-2168\"\r\n  },\r\n  {\r\n    \"customerId\": \"ANTON\",\r\n    \"companyName\": \"Antonio Moreno Taquería\",\r\n    \"contactName\": \"Antonio Moreno\",\r\n    \"contactTitle\": \"Owner\",\r\n    \"address\": \"Mataderos  2312\",\r\n    \"city\": \"México D.F.\",\r\n    \"region\": null,\r\n    \"postalCode\": \"05023\",\r\n    \"country\": \"Mexico\",\r\n    \"phone\": \"(5) 555-3932\",\r\n    \"fax\": null\r\n  },\r\n  {\r\n    \"customerId\": \"FRANR\",\r\n    \"companyName\": \"France restauration\",\r\n    \"contactName\": \"Carine Schmitt\",\r\n    \"contactTitle\": \"Marketing Manager\",\r\n    \"address\": \"54, rue Royale\",\r\n    \"city\": \"Nantes\",\r\n    \"region\": null,\r\n    \"postalCode\": \"44000\",\r\n    \"country\": \"France\",\r\n    \"phone\": \"40.32.21.21\",\r\n    \"fax\": \"40.32.21.20\"\r\n  },\r\n  {\r\n    \"customerId\": \"SPECD\",\r\n    \"companyName\": \"Spécialités du monde\",\r\n    \"contactName\": \"Dominique Perrier\",\r\n    \"contactTitle\": \"Marketing Manager\",\r\n    \"address\": \"25, rue Lauriston\",\r\n    \"city\": \"Paris\",\r\n    \"region\": null,\r\n    \"postalCode\": \"75016\",\r\n    \"country\": \"France\",\r\n    \"phone\": \"(1) 47.55.60.10\",\r\n    \"fax\": \"(1) 47.55.60.20\"\r\n  },\r\n  {\r\n    \"customerId\": \"PERIC\",\r\n    \"companyName\": \"Pericles Comidas clásicas\",\r\n    \"contactName\": \"Guillermo Fernández\",\r\n    \"contactTitle\": \"Sales Representative\",\r\n    \"address\": \"Calle Dr. Jorge Cash 321\",\r\n    \"city\": \"México D.F.\",\r\n    \"region\": null,\r\n    \"postalCode\": \"05033\",\r\n    \"country\": \"Mexico\",\r\n    \"phone\": \"(5) 552-3745\",\r\n    \"fax\": \"(5) 545-3745\"\r\n  },\r\n  {\r\n    \"customerId\": \"BLAUS\",\r\n    \"companyName\": \"Blauer See Delikatessen\",\r\n    \"contactName\": \"Hanna Moos\",\r\n    \"contactTitle\": \"Sales Representative\",\r\n    \"address\": \"Forsterstr. 57\",\r\n    \"city\": \"Mannheim\",\r\n    \"region\": null,\r\n    \"postalCode\": \"68306\",\r\n    \"country\": \"Germany\",\r\n    \"phone\": \"0621-08460\",\r\n    \"fax\": \"0621-08924\"\r\n  },\r\n  {\r\n    \"customerId\": \"SEVES\",\r\n    \"companyName\": \"Seven Seas Imports\",\r\n    \"contactName\": \"Hari Kumar\",\r\n    \"contactTitle\": \"Sales Manager\",\r\n    \"address\": \"90 Wadhurst Rd.\",\r\n    \"city\": \"London\",\r\n    \"region\": null,\r\n    \"postalCode\": \"OX15 4NB\",\r\n    \"country\": \"UK\",\r\n    \"phone\": \"(171) 555-1717\",\r\n    \"fax\": \"(171) 555-5646\"\r\n  },\r\n  {\r\n    \"customerId\": \"OTTIK\",\r\n    \"companyName\": \"Ottilies Käseladen\",\r\n    \"contactName\": \"Henriette Pfalzheim\",\r\n    \"contactTitle\": \"Owner\",\r\n    \"address\": \"Mehrheimerstr. 369\",\r\n    \"city\": \"Köln\",\r\n    \"region\": null,\r\n    \"postalCode\": \"50739\",\r\n    \"country\": \"Germany\",\r\n    \"phone\": \"0221-0644327\",\r\n    \"fax\": \"0221-0765721\"\r\n  },\r\n  {\r\n    \"customerId\": \"LETSS\",\r\n    \"companyName\": \"Let's Stop N Shop\",\r\n    \"contactName\": \"Jaime Yorres\",\r\n    \"contactTitle\": \"Owner\",\r\n    \"address\": \"87 Polk St. Suite 5\",\r\n    \"city\": \"San Francisco\",\r\n    \"region\": \"CA\",\r\n    \"postalCode\": \"94117\",\r\n    \"country\": \"USA\",\r\n    \"phone\": \"(415) 555-5938\",\r\n    \"fax\": null\r\n  },\r\n  {\r\n    \"customerId\": \"RICAR\",\r\n    \"companyName\": \"Ricardo Adocicados\",\r\n    \"contactName\": \"Janete Limeira\",\r\n    \"contactTitle\": \"Assistant Sales Agent\",\r\n    \"address\": \"Av. Copacabana, 267\",\r\n    \"city\": \"Rio de Janeiro\",\r\n    \"region\": \"RJ\",\r\n    \"postalCode\": \"02389-890\",\r\n    \"country\": \"Brazil\",\r\n    \"phone\": \"(21) 555-3412\",\r\n    \"fax\": null\r\n  },\r\n  {\r\n    \"customerId\": \"GROSR\",\r\n    \"companyName\": \"GROSELLA-Restaurante\",\r\n    \"contactName\": \"Manuel Pereira\",\r\n    \"contactTitle\": \"Owner\",\r\n    \"address\": \"5ª Ave. Los Palos Grandes\",\r\n    \"city\": \"Caracas\",\r\n    \"region\": \"DF\",\r\n    \"postalCode\": \"1081\",\r\n    \"country\": \"Venezuela\",\r\n    \"phone\": \"(2) 283-2951\",\r\n    \"fax\": \"(2) 283-3397\"\r\n  },  \r\n  {\r\n    \"customerId\": \"FOLKO\",\r\n    \"companyName\": \"Folk och fä HB\",\r\n    \"contactName\": \"Maria Larsson\",\r\n    \"contactTitle\": \"Owner\",\r\n    \"address\": \"Åkergatan 24\",\r\n    \"city\": \"Bräcke\",\r\n    \"region\": null,\r\n    \"postalCode\": \"S-844 67\",\r\n    \"country\": \"Sweden\",\r\n    \"phone\": \"0695-34 67 21\",\r\n    \"fax\": null\r\n  },\r\n  {\r\n    \"customerId\": \"PARIS\",\r\n    \"companyName\": \"Paris spécialités\",\r\n    \"contactName\": \"Marie Bertrand\",\r\n    \"contactTitle\": \"Owner\",\r\n    \"address\": \"265, boulevard Charonne\",\r\n    \"city\": \"Paris\",\r\n    \"region\": null,\r\n    \"postalCode\": \"75012\",\r\n    \"country\": \"France\",\r\n    \"phone\": \"(1) 42.34.22.66\",\r\n    \"fax\": \"(1) 42.34.22.77\"\r\n  },\r\n  {\r\n    \"customerId\": \"HANAR\",\r\n    \"companyName\": \"Hanari Carnes\",\r\n    \"contactName\": \"Mario Pontes\",\r\n    \"contactTitle\": \"Accounting Manager\",\r\n    \"address\": \"Rua do Paço, 67\",\r\n    \"city\": \"Rio de Janeiro\",\r\n    \"region\": \"RJ\",\r\n    \"postalCode\": \"05454-876\",\r\n    \"country\": \"Brazil\",\r\n    \"phone\": \"(21) 555-0091\",\r\n    \"fax\": \"(21) 555-8765\"\r\n  },\r\n  {\r\n    \"customerId\": \"BOLID\",\r\n    \"companyName\": \"Bólido Comidas preparadas\",\r\n    \"contactName\": \"Martín Sommer\",\r\n    \"contactTitle\": \"Owner\",\r\n    \"address\": \"C/ Araquil, 67\",\r\n    \"city\": \"Madrid\",\r\n    \"region\": null,\r\n    \"postalCode\": \"28023\",\r\n    \"country\": \"Spain\",\r\n    \"phone\": \"(91) 555 22 82\",\r\n    \"fax\": \"(91) 555 91 99\"\r\n  },\r\n  {\r\n    \"customerId\": \"FOLIG\",\r\n    \"companyName\": \"Folies gourmandes\",\r\n    \"contactName\": \"Martine Rancé\",\r\n    \"contactTitle\": \"Assistant Sales Agent\",\r\n    \"address\": \"184, chaussée de Tournai\",\r\n    \"city\": \"Lille\",\r\n    \"region\": null,\r\n    \"postalCode\": \"59000\",\r\n    \"country\": \"France\",\r\n    \"phone\": \"20.16.10.16\",\r\n    \"fax\": \"20.16.10.17\"\r\n  },\r\n  {\r\n    \"customerId\": \"VICTE\",\r\n    \"companyName\": \"Victuailles en stock\",\r\n    \"contactName\": \"Mary Saveley\",\r\n    \"contactTitle\": \"Sales Agent\",\r\n    \"address\": \"2, rue du Commerce\",\r\n    \"city\": \"Lyon\",\r\n    \"region\": null,\r\n    \"postalCode\": \"69004\",\r\n    \"country\": \"France\",\r\n    \"phone\": \"78.32.54.86\",\r\n    \"fax\": \"78.32.54.87\"\r\n  },\r\n  {\r\n    \"customerId\": \"WILMK\",\r\n    \"companyName\": \"Wilman Kala\",\r\n    \"contactName\": \"Matti Karttunen\",\r\n    \"contactTitle\": \"Owner/Marketing Assistant\",\r\n    \"address\": \"Keskuskatu 45\",\r\n    \"city\": \"Helsinki\",\r\n    \"region\": null,\r\n    \"postalCode\": \"21240\",\r\n    \"country\": \"Finland\",\r\n    \"phone\": \"90-224 8858\",\r\n    \"fax\": \"90-224 8858\"\r\n  }\r\n]";
        private void SeedJsonDta(NorthWindsContext dbContext)
        {
            List<NorthWinds.Persistence.Entities.Customer> customers = JsonSerializer.Deserialize<List<NorthWinds.Persistence.Entities.Customer>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            Assert.NotNull(customers);
            Assert.NotEmpty(customers);            
            dbContext.Customers.AddRange(customers);
            Assert.Equal(19, dbContext.SaveChanges());
        }

        [Fact]
        public async Task Get_CustomerbySearch_Test()
        {
            //Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<NorthWindsContext>();
                SeedJsonDta(dbContext);
            }


            string partialName = "m";
            string partialNumber = "";

            var client = _factory.CreateClient();

            //Act

            var response = await client.GetAsync($"/api/Customer?partialName={partialName}&partialPhoneNumber={partialNumber}&pageNumber=1&resultsPerPage=10");            

            var responseContent = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent);
            List<Customer> customers = JsonSerializer.Deserialize<List<Customer>>(responseContent)!;

            Assert.NotNull(customers);
            Assert.Equal(10, customers.Count);
            Assert.All(customers, customer => Assert.Contains(partialName, customer.ContactName));

        }

        [Fact]
        public async Task Post_CreatCustomer_Test()
        {
            //Arrange
            var customer = new Customer
            {
                CustomerId = "test1",
                CompanyName = "fake company",
                ContactName = "John Doe",
                Address = "123 Fake st",
                City = "Fake city",
                PostalCode = "90210",
                Phone = "8888675309",
                ContactTitle = "My Liege",
                Country = "USA",
                Fax="Nobody faxes",
                Region = "West"
            };

            var client = _factory.CreateClient();

            //Act

            var response = await client.PostAsJsonAsync($"/api/Customer", customer);

            var responseContent = await response.Content.ReadAsStringAsync();

            //Assert

            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

            Assert.NotNull(responseContent);
            var savedCustomer = JsonSerializer.Deserialize<Customer>(responseContent);            

            Assert.Equal(customer, savedCustomer);
            

        }

        [Fact]
        public async Task Post_CreatCustomer_Validation_Fail_Test()
        {
            //Arrange
            var customer = new Customer
            {
                CustomerId = "test1",
                CompanyName = "fake company with way to long a name that won't isn't permitted.",
                ContactName = "John Doe",
                Address = "123 Fake st",
                City = "Fake city",
                PostalCode = "90210",
                Phone = "8888675309",
                ContactTitle = "My Liege",
                Country = "USA",
                Fax = "Nobody faxes",
                Region = "West"
            };

            var client = _factory.CreateClient();

            //Act

            var response = await client.PostAsJsonAsync($"/api/Customer", customer);

            var responseContent = await response.Content.ReadAsStringAsync();

            //Assert

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

            Assert.NotNull(responseContent);

            Assert.Contains("The field CompanyName must be a string with a maximum length of 40", responseContent);
            
        }

        [Fact]
        public async Task Put_UpdateCustomer_EntityNotPresent_Test()
        {
            //Arrange
            var customer = new Customer
            {
                CustomerId = "tes99",
                CompanyName = "fake company",
                ContactName = "John Doe",
                Address = "123 Fake st",
                City = "Fake city",
                PostalCode = "90210",
                Phone = "8888675309",
                ContactTitle = "My Liege",
                Country = "USA",
                Fax = "Nobody faxes",
                Region = "West"
            };

            var client = _factory.CreateClient();           

            //Act

            var response = await client.PutAsJsonAsync($"/api/Customer", customer);

            var responseContent = await response.Content.ReadAsStringAsync();

            //Assert

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

            Assert.NotNull(responseContent);

            Assert.Contains($"\"Entity with {customer.CustomerId} is not present\"", responseContent);
        }

        [Fact]
        public async Task Put_UpdateCustomer_Test()
        {
            //Arrange
            var customer = new Customer
            {
                CustomerId = "cus99",
                CompanyName = "fake company",
                ContactName = "John Doe",
                Address = "123 Fake st",
                City = "Fake city",
                PostalCode = "90210",
                Phone = "8888675309",
                ContactTitle = "My Liege",
                Country = "USA",
                Fax = "Nobody faxes",
                Region = "West"
            };

            var client = _factory.CreateClient();

            var create_response = await client.PostAsJsonAsync($"/api/Customer", customer);
            Assert.Equal(System.Net.HttpStatusCode.Created, create_response.StatusCode);

            customer.ContactTitle = "deposed";

            //Act

            var response = await client.PutAsJsonAsync($"/api/Customer", customer);

            var responseContent = await response.Content.ReadAsStringAsync();

            //Assert

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            Assert.NotNull(responseContent);

            var updatedCustomer = JsonSerializer.Deserialize<Customer>(responseContent);

            Assert.Equal(customer, updatedCustomer);
        }


    }
}
