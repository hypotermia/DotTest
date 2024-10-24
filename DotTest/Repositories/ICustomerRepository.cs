using DotTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotTest.Repositories
{
    
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(int id);
        Task AddCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(int id);
    }

}
