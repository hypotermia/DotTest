using DotTest.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotTest.Services
{

    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task<OrderDto> GetOrderById(int id);
        Task AddOrder(OrderDto orderDto);
        Task UpdateOrder(OrderDto orderDto);
        Task DeleteOrder(int id);
    }

}
