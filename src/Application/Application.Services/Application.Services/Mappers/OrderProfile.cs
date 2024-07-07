using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<NorthWinds.Persistence.Entities.Order, Models.DTOs.Order.Order>();
            CreateMap<NorthWinds.Persistence.Entities.OrderDetail, Models.DTOs.Order.OrderDetail>();
            CreateMap<NorthWinds.Persistence.Entities.Product, Models.DTOs.Order.Product>();
            CreateMap<NorthWinds.Persistence.Entities.Shipper, Models.DTOs.Order.Shipper>();
        }
    }
}
