using AutoMapper;
using DotTest.DTOs;
using DotTest.Models;
using DotTest.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DotTest.Services
{


    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await _repository.GetAllOrders();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderById(int id)
        {
            var order = await _repository.GetOrderById(id);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task AddOrder(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _repository.AddOrder(order);
        }

        public async Task UpdateOrder(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _repository.UpdateOrder(order);
        }

        public async Task DeleteOrder(int id)
        {
            await _repository.DeleteOrder(id);
        }
    }

}
