using DotTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotTest.Repositories
{
    
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DotTestContext _context;

        public CustomerRepository(DotTestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _context.Customers.Include(c => c.Orders).ToListAsync(); // Eager load Orders
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await _context.Customers.Include(c => c.Orders).FirstOrDefaultAsync(c => c.CustomerId == id); // Eager load Orders
        }

        public async Task AddCustomer(Customer customer)
        {

            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
            {
                throw new ArgumentException("Customer with this email already exists.");
            }
            if (await _context.Customers.AnyAsync(c => c.Name == customer.Name))
            {
                throw new ArgumentException("Customer with this name already exists.");
            }
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomer(Customer customer)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
            {
                throw new ArgumentException("Customer with this email already exists.");
            }
            if (await _context.Customers.AnyAsync(c => c.Name == customer.Name))
            {
                throw new ArgumentException("Customer with this email already exists.");
            }
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }

}
