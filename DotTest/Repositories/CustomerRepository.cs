using DotTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotTest.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DotTestContext _context;
        private readonly IMemoryCache _cache;

        public CustomerRepository(DotTestContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            // Cek apakah data ada di cache
            if (!_cache.TryGetValue("customers", out IEnumerable<Customer> customers))
            {
                // Jika tidak ada, ambil dari database
                customers = await _context.Customers.ToListAsync();

                // Simpan hasil ke cache
                _cache.Set("customers", customers, TimeSpan.FromMinutes(5)); // Cache selama 5 menit
            }

            return customers;
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var cacheKey = $"customer_{id}";

            // Cek apakah data ada di cache
            if (!_cache.TryGetValue(cacheKey, out Customer customer))
            {
                // Jika tidak ada, ambil dari database
                customer = await _context.Customers.FindAsync(id);

                if (customer != null)
                {
                    // Simpan hasil ke cache
                    _cache.Set(cacheKey, customer, TimeSpan.FromMinutes(5)); // Cache selama 5 menit
                }
            }

            return customer;
        }

        public async Task AddCustomer(Customer customer)
        {
            // Validasi (sama seperti sebelumnya)

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Invalidate cache setelah menambah data
            _cache.Remove("customers"); // Menghapus cache
        }

        public async Task UpdateCustomer(Customer customer)
        {
            // Validasi (sama seperti sebelumnya)

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            // Invalidate cache setelah memperbarui data
            _cache.Remove($"customer_{customer.CustomerId}");
            _cache.Remove("customers"); // Menghapus cache
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await GetCustomerById(id);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            // Invalidate cache setelah menghapus data
            _cache.Remove($"customer_{id}");
            _cache.Remove("customers"); // Menghapus cache
        }
    }


}
