using Application.Services;
using AutoMapper;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using NorthWinds.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;


        public CustomerService(ILogger<CustomerService> logger, ICustomerRepository customerRepository, IMapper mapper)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<Application.Models.DTOs.Customer.Customer> AddCustomerAsync(Application.Models.DTOs.Customer.Customer customer)
        {
            var customer_ent = _mapper.Map<Customer>(customer);
            var enity =  _customerRepository.Add(customer_ent);
            await _customerRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<Application.Models.DTOs.Customer.Customer>(enity);
        }

        public async Task<Application.Models.DTOs.Customer.Customer?> GetCustomerAsync(string customerid)
        {
            var customer = await _customerRepository.GetAsync(customerid);
            return _mapper.Map<Application.Models.DTOs.Customer.Customer>(customer);
        }

        public async Task<IEnumerable<Application.Models.DTOs.Customer.Customer>> GetCustomersAsync(string? partialName = null, string? partialPhoneNumber = null, int pageNumber = 1, int resultsPerPage = 10)
        {
            var customer_Entity = await _customerRepository.GetCustomersAsync(partialName, partialPhoneNumber, pageNumber, resultsPerPage);
            return _mapper.Map<IEnumerable<Application.Models.DTOs.Customer.Customer>>(customer_Entity);
        }

        public async Task<Application.Models.DTOs.Customer.Customer> UpdateCustomerAsync(Application.Models.DTOs.Customer.Customer customer)
        {
            var existingRecord = await _customerRepository.GetAsync(customer.CustomerId);
            if (existingRecord == null) throw new ArgumentException($"Entity with {customer.CustomerId} is not present");

            var customer_ent = _mapper.Map<Customer>(customer);
            customer_ent = _customerRepository.Update(customer_ent);
            await _customerRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<Application.Models.DTOs.Customer.Customer>(customer_ent);
        }
    }
}
