using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NorthWinds.Persistence.DBContexts;
using NorthWinds.Persistence.Entities;
using NorthWinds.Persistence.Repositories;


namespace NorthWinds.Persistence.Test
{
    public class CustomerRepositoryTests 
    {

        private readonly Mock<ILogger<CustomerRepository>> _loggerMock;
        private readonly CustomerRepository _repository;
        protected readonly NorthWindsContext _context;

        public CustomerRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<NorthWindsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new NorthWindsContext(options);

            _loggerMock = new Mock<ILogger<CustomerRepository>>();
            _repository = new CustomerRepository(_loggerMock.Object, _context);

        }

        [Fact]
        public async Task GetAsync_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            string testCustomerId = "TestId";
            var testCustomer = new Customer { CustomerId = testCustomerId, ContactName = "John Doe", Phone = "123-456-7890", CompanyName = "fake company" };
            _context.Customers.Add(testCustomer);

            await _context.SaveChangesAsync();


            // Act
            var result = await _repository.GetAsync(testCustomerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testCustomerId, result.CustomerId);
        }

        [Fact]
        public async Task GetAsync_ReturnsCustomerByContactName_WhenCustomerExists()
        {
            // Arrange
            string testCustomerId = "TestId2";
            string testContactName = "John Conner";
            var testCustomer = new Customer { CustomerId = testCustomerId, ContactName = testContactName, Phone = "123-456-7890", CompanyName = "fake company" };

            _context.Customers.Add(testCustomer);

            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByContactNameAsync(testContactName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testCustomerId, result.CustomerId);
            Assert.Equal(testContactName, result.ContactName);
        }


        [Fact]
        public async Task GetCustomersAsync_Returns_Customers_With_PartialName3()
        {
            //Arange
            _context.Customers.AddRange(new List<Customer>
            {
                new Customer {CustomerId="1", ContactName = "John Doe", Phone = "123-456-7890", CompanyName="fake company" },
                new Customer {CustomerId="2", ContactName = "Jane Doe", Phone = "234-567-8901", CompanyName="fake company" },
                new Customer {CustomerId="3", ContactName = "Jack Smith", Phone = "345-678-9012", CompanyName="fake company" },
            });

            await _context.SaveChangesAsync();

            // Act
            var results = await _repository.GetCustomersAsync(partialName: "Doe");

            // Assert
            Assert.Equal(2, results.Count());
            Assert.Contains(results, c => c.ContactName == "John Doe");
            Assert.Contains(results, c => c.ContactName == "Jane Doe");

        }

    }
}