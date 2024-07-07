using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NorthWinds.Persistence.DBContexts;
using NorthWinds.Persistence.Entities;
using NorthWinds.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NorthWinds.Persistence.Test
{
    public class ReadOnlyOrdersRepositoryTests
    {
        private readonly Mock<ILogger<ReadOnlyOrdersRepository>> _loggerMock;
        private readonly IReadOnlyOrdersRepository _repository;
        private readonly NorthWindsReadOnlyContext _context;
        private readonly NorthWindsContext _rwContext;

        private static readonly InMemoryDatabaseRoot InMemoryDatabaseRoot = new InMemoryDatabaseRoot();

        public ReadOnlyOrdersRepositoryTests()
        {
            var databaseName = Guid.NewGuid().ToString();

           
            var options = new DbContextOptionsBuilder<NorthWindsReadOnlyContext>()
               .UseInMemoryDatabase(databaseName, InMemoryDatabaseRoot)
               .Options;

            var rwOptions = new DbContextOptionsBuilder<NorthWindsContext>()
               .UseInMemoryDatabase(databaseName, InMemoryDatabaseRoot)               
               .Options;
         

            _context = new NorthWindsReadOnlyContext(options);
            _rwContext = new NorthWindsContext(rwOptions);

            _loggerMock = new Mock<ILogger<ReadOnlyOrdersRepository>>();
            _repository = new ReadOnlyOrdersRepository(_loggerMock.Object, _context);

            _context.Database.EnsureCreated();
            _rwContext.Database.EnsureCreated();
        }       

        [Fact]
        public async Task GetOrderAsync_test()
        {
            // Arrange
            int orderId = 1;
            var testOrder = new Order { OrderId = orderId,  CustomerId = "12345", Freight=30.5m, ShipVia=1 };          
            _rwContext.Orders.Add(testOrder);

            await _rwContext.SaveChangesAsync();            

            // Act
            var result = await _repository.GetOrderAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);
        }

        [Fact]
        public async Task GetOrdersByEmployeeIdAsync_test()
        {
            // Arrange
            int employeeId = 5;
            var testOrder = new Order { OrderId = 1, CustomerId = "12345", Freight = 30.5m, ShipVia = 1, EmployeeId=employeeId };
            _rwContext.Orders.Add(testOrder);

            await _rwContext.SaveChangesAsync();

            // Act
            var results = await _repository.GetOrdersByEmployeeIdAsync(employeeId);

            // Assert
            Assert.NotNull(results);
            Assert.All(results, x => Assert.Equal(employeeId, x.EmployeeId));
        }

        [Fact]
        public async Task GetOrdersByCustomerIdAsync_test()
        {
            // Arrange
            int employeeId = 5;
            string customerId = "12345";
            var testOrder = new Order { OrderId = 1, CustomerId = customerId, Freight = 30.5m, ShipVia = 1, EmployeeId = employeeId };
            _rwContext.Orders.Add(testOrder);

            await _rwContext.SaveChangesAsync();

            // Act
            var results = await _repository.GetOrdersByCustomerIdAsync(customerId, 1, 10);

            // Assert
            Assert.NotNull(results);
            Assert.All(results, x => Assert.Equal(customerId, x.CustomerId));
        }




    }
}
