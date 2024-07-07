using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NorthWinds.Persistence.DBContexts;
using NorthWinds.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace NorthWinds.Persistence.Repositories
{
    public class ReadOnlyOrdersRepository : IReadOnlyOrdersRepository
    {
        private readonly ILogger<ReadOnlyOrdersRepository> _logger;
        private readonly NorthWindsReadOnlyContext _context;


        public ReadOnlyOrdersRepository(ILogger<ReadOnlyOrdersRepository> logger, NorthWindsReadOnlyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Order?> GetOrderAsync(int OrderId)
        {
            Order? order= await _context.Orders
                .AsNoTracking()
                .SingleOrDefaultAsync(o => o.OrderId == OrderId);

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId, int pageNumber = 1, int resultsPerPage = 10)
        {
            var query = _context.Orders
                            .Where(o => o.CustomerId == customerId)
                            .OrderBy(o=>o.OrderDate)
                            .Skip((pageNumber - 1) * resultsPerPage)
                            .Take(resultsPerPage);


            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByEmployeeIdAsync(int EmployeeId, int pageNumber = 1, int resultsPerPage = 10)
        {
            var query = _context.Orders
                             .Where(o => o.EmployeeId == EmployeeId)
                             .OrderBy(o => o.OrderDate)
                             .Skip((pageNumber - 1) * resultsPerPage)
                             .Take(resultsPerPage);


            return await query.ToListAsync();
        }
    }
}
