using AutoMapper;
using Ecommerce.Dtos.User.GetInTouch_About;
using Ecommerce.Models.User.AboutAndContact;

namespace Ecommerce.Helpers.AutoMapperFiles
{
    public class About_GetInTouchMap:Profile
    {
        public About_GetInTouchMap()
        {
            CreateMap<AboutModel, AboutDto>().ReverseMap();
            CreateMap<GetinTouchModel, GetInTouchDto>().ReverseMap();
            CreateMap<SendMessageDto, GetinTouchModel>();
        }
    }
}
