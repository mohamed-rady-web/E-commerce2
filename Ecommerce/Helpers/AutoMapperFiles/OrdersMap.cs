using AutoMapper;
using Ecommerce.Dtos.Orders;
using Ecommerce.Models.Order;

namespace Ecommerce.Mappings
{
    public class OrdersMap : Profile
    {
        public OrdersMap()
        {
        
            CreateMap<OrderModel, OrderDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Items != null ? src.Items.Sum(i => i.Quantity) : 0))
                .ReverseMap();

       
            CreateMap<OrderItemModel, OrderItemDto>().ReverseMap();

         
            CreateMap<CreateOrderDto, OrderModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

          
            CreateMap<AddOrderItemDto, OrderItemModel>();

         
            CreateMap<UpdateOrderDto, OrderModel>()
                .ForMember(dest => dest.Items, opt => opt.Ignore()); 
        }
    }
}
