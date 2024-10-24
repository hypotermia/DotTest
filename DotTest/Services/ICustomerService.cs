using DotTest.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotTest.Services
{

    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomers();
        Task<CustomerDto> GetCustomerById(int id);
        Task AddCustomer(CustomerDto customerDto);
        Task UpdateCustomer(CustomerDto customerDto);
        Task DeleteCustomer(int id);
    }

}
