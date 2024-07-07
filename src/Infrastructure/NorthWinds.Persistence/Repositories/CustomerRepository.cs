using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NorthWinds.Persistence.DBContexts;
using NorthWinds.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWinds.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly ILogger<CustomerRepository> _logger;
        private readonly NorthWindsContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public CustomerRepository(ILogger<CustomerRepository> logger, NorthWindsContext context)
        {
            _logger = logger;
            _context = context;
        }


        public Customer Add(Customer customer)
        {
            return _context.Customers.Add(customer).Entity;
        }

        public async Task<Customer?> GetAsync(string CustomerId)
        {
            var customer =  await _context.Customers                                 
                                 .AsNoTracking()
                                 .SingleOrDefaultAsync(c => c.CustomerId == CustomerId);

            if (customer == null) return null;

            return customer;

        }

        public async Task<Customer?> GetByContactNameAsync(string ContactName)
        {
            var customer = await _context.Customers
                    .Where(c => c.ContactName == ContactName)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();

            if (customer == null) return null;

            return customer;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(string? partialName = null, string? partialPhoneNumber = null, int pageNumber = 1, int resultsPerPage = 10)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(partialName))
            {
                query = query.Where(c => c.ContactName != null && c.ContactName.Contains(partialName));
            }

            if (!string.IsNullOrEmpty(partialPhoneNumber))
            {
                query = query.Where(c => c.Phone != null && c.Phone.Contains(partialPhoneNumber));
            }

            query = query.OrderBy(c => c.ContactName)
                         .Skip((pageNumber - 1) * resultsPerPage)
                         .Take(resultsPerPage);

            return await query.ToListAsync();
        }

        public Customer Update(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            return customer;
        }
    }
}
