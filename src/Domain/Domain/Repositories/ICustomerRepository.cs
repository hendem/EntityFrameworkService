using NorthWinds.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public  interface ICustomerRepository: IRepository
    {
        Task<Customer?> GetAsync(string CustomerId);
        Task<Customer?> GetByContactNameAsync(string CustomerName);

        Task<IEnumerable<Customer>> GetCustomersAsync(string? partialName = null, string? partialPhoneNumber = null, int pageNumber = 1, int resultsPerPage = 10);

        Customer Add(Customer customer);
        Customer Update(Customer customer);

        
    }
}
