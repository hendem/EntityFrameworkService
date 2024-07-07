using Application.Models.DTOs.Customer;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Services
{
    public interface ICustomerService
    {
        public Task<Customer?> GetCustomerAsync(string customerid);
        public Task<IEnumerable<Customer>> GetCustomersAsync(string? partialName = null, string? partialPhoneNumber = null, int pageNumber = 1, int resultsPerPage = 10);

        public Task<Customer> AddCustomerAsync(Application.Models.DTOs.Customer.Customer customer);
        public Task<Customer> UpdateCustomerAsync(Customer customer);
    }
}