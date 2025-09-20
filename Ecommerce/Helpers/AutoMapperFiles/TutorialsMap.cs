using AutoMapper;
using Ecommerce.Dtos.User.TutorialsAndMaintenance;
using Ecommerce.Models.User.ServicesAndTutorials;

namespace Ecommerce.Helpers.AutoMapperFiles
{
    public class TutorialsMap : Profile
    {
        public TutorialsMap()
        {
            CreateMap<AddTutorialDto, TutorialsModel>();
            CreateMap<UpdateTutorialDto, TutorialsModel>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<TutorialsModel, TutorialsDto>();


        }
    }
}
