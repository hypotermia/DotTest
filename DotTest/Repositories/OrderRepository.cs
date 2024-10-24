using DotTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DotTest.Repositories
{

    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public OrderRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            // Cek apakah data ada di cache
            if (!_cache.TryGetValue("orders", out IEnumerable<Order> orders))
            {
                // Jika tidak ada, ambil dari database
                orders = await _context.Orders.Include(o => o.Customer).ToListAsync();

                // Simpan hasil ke cache
                _cache.Set("orders", orders, TimeSpan.FromMinutes(5)); // Cache selama 5 menit
            }

            return orders;
        }

        public async Task<Order> GetOrderById(int id)
        {
            var cacheKey = $"order_{id}";

            // Cek apakah data ada di cache
            if (!_cache.TryGetValue(cacheKey, out Order order))
            {
                // Jika tidak ada, ambil dari database
                order = await _context.Orders.Include(o => o.Customer).FirstOrDefaultAsync(o => o.OrderId == id);

                if (order != null)
                {
                    // Simpan hasil ke cache
                    _cache.Set(cacheKey, order, TimeSpan.FromMinutes(5)); // Cache selama 5 menit
                }
            }

            return order;
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
            _cache.Remove("orders");
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
            _cache.Remove($"order_{order.OrderId}");
            _cache.Remove("orders"); // Menghapus cache
        }

        public async Task DeleteOrder(int id)
        {
            var order = await GetOrderById(id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            // Invalidate cache setelah menghapus data
            _cache.Remove($"order_{id}");
            _cache.Remove("orders"); // Menghapus cache
        }
    }

}
