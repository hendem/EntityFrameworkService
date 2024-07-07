using Application.Models.DTOs.Order;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IReadOnlyOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;
        public OrderService(ILogger<OrderService> logger, IReadOnlyOrdersRepository repository, IMapper mapper)
        {
            _logger = logger;
            _ordersRepository = repository;
            _mapper = mapper;
        }


        public async Task<Order?> GetOrderAsync(int OrderId)
        {
            var order = await _ordersRepository.GetOrderAsync(OrderId);
            return _mapper.Map<Order>(order);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId, int pageNumber = 1, int resultsPerPage = 10)
        {
            var orders = await _ordersRepository.GetOrdersByCustomerIdAsync(customerId, pageNumber, resultsPerPage);
            return _mapper.Map<IEnumerable<Order>>(orders);
        }

        public async Task<IEnumerable<Order>> GetOrdersByEmployeeIdAsync(int EmployeeId, int pageNumber = 1, int resultsPerPage = 10)
        {
            var orders = await _ordersRepository.GetOrdersByEmployeeIdAsync(EmployeeId, pageNumber, resultsPerPage);
            return _mapper.Map<IEnumerable<Order>>(orders);
        }
    }
}
