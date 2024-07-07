using NorthWinds.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IReadOnlyOrdersRepository
    {
        public Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId, int pageNumber = 1, int resultsPerPage = 10);

        public Task<IEnumerable<Order>> GetOrdersByEmployeeIdAsync(int EmployeeId, int pageNumber = 1, int resultsPerPage = 10);

        public Task<Order?> GetOrderAsync(int OrderId);

    }
}
