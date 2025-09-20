using AutoMapper;
using Ecommerce.Dtos.User.TutorialsAndMaintenance;
using Ecommerce.Models.User.ServicesAndTutorials;

namespace Ecommerce.Helpers.AutoMapperFiles
{
    public class MaintenanceServiceMap : Profile
    {
        public MaintenanceServiceMap()
        {
            CreateMap<MaintenanceServicesModel, MaintenanceServiceDto>();
            CreateMap<MaintenanceServiceDto, MaintenanceServicesModel>();
        }
    }
}
