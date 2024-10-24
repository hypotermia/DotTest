using AutoMapper;
using DotTest.DTOs;
using DotTest.Models;

namespace DotTest.Services
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Pemetaan untuk Customer
            CreateMap<Customer, CustomerDto>().ReverseMap();

            // Pemetaan untuk Order
            CreateMap<Order, OrderDto>().ReverseMap();
        }
    }

}
