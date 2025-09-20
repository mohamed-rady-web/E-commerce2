using AutoMapper;
using Ecommerce.Dtos.User.Reservation;
using Ecommerce.Models.User.FavAndBooking;

namespace Ecommerce.Helpers.AutoMapperFiles
{
    public class BookingMap : Profile
    {
        public BookingMap()
        {
            CreateMap<BookingModel, BookingDto>()
               .ForMember(dest => dest.Message, opt => opt.Ignore()) 
               .ForMember(dest => dest.AvaliableDates, opt => opt.MapFrom(src => src.AvaliableDates))
               .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.Services));

            CreateMap<AddBookingDto, BookingModel>();
            CreateMap<UpdateBookingDto, BookingModel>();

          
            CreateMap<AvaliableDatesModel, AddAvailableDatesDto>().ReverseMap();

            
            CreateMap<BookingDto, BookingModel>()
                .ForMember(dest => dest.AvaliableDates, opt => opt.MapFrom(src => src.AvaliableDates))
                .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Service));
        }
    }
}
