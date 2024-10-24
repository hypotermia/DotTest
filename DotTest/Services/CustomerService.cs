using AutoMapper;
using DotTest.DTOs;
using DotTest.Models;
using DotTest.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DotTest.Services
{


    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
        {
            var customers = await _repository.GetAllCustomers();
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> GetCustomerById(int id)
        {
            var customer = await _repository.GetCustomerById(id);
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task AddCustomer(CustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            await _repository.AddCustomer(customer);
        }

        public async Task UpdateCustomer(CustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            await _repository.UpdateCustomer(customer);
        }

        public async Task DeleteCustomer(int id)
        {
            await _repository.DeleteCustomer(id);
        }
    }

}
