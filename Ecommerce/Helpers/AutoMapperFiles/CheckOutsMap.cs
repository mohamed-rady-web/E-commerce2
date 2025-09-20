using AutoMapper;
using Ecommerce.Dtos.CheckOut;
using Ecommerce.Dtos.Orders;
using Ecommerce.Dtos.User.Reservation;
using Ecommerce.Dtos.User.Reviews;
using Ecommerce.Models.Order;
using Ecommerce.Models.User.AboutAndContact;
using Ecommerce.Models.User.FavAndBooking;

public class CheckOutProfile : Profile
{
    public CheckOutProfile()
    {

        CreateMap<CheckOutModel, CheckOutDto>();
        CreateMap<OrderModel, OrderDto>();
        CreateMap<ReviewsModel, ReviewsDto>()
            .ForMember(dest => dest.Productname, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null));
        CreateMap<BookingModel, BookingDto>();
    }
}
