using AutoMapper;
using Domain.Dto;

namespace Domain.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<AuditLogDto, AuditLog>()
            //    .ReverseMap();
            CreateMap<Door, DoorDto>();
        }
    }
}
