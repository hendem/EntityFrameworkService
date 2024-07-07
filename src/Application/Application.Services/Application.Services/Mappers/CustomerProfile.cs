using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Models.DTOs.Customer.Customer, NorthWinds.Persistence.Entities.Customer>().ReverseMap();
        }
        

    }
}
