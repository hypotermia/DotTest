using DotTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotTest.Repositories
{

    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(int id);
    }

}
