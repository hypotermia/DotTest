using DotTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DotTest.Repositories
{


    public class OrderRepository : IOrderRepository
    {
        private readonly DotTestContext _context;

        public OrderRepository(DotTestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.Include(o => o.Customer).ToListAsync(); // Eager load Customer
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.Include(o => o.Customer).FirstOrDefaultAsync(o => o.OrderId == id); // Eager load Customer
        }

        public async Task AddOrder(Order order)
        {
            if (order.CustomerId <= 0 || await _context.Customers.FindAsync(order.CustomerId) == null)
            {
                throw new ArgumentException("Invalid customer ID.");
            }

            if (order.Price < 0)
            {
                throw new ArgumentException("Total price cannot be negative.");
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            if (order.CustomerId <= 0 || await _context.Customers.FindAsync(order.CustomerId) == null)
            {
                throw new ArgumentException("Invalid customer ID.");
            }


            if (order.Price < 0)
            {
                throw new ArgumentException("Total price cannot be negative.");
            }

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }

}
