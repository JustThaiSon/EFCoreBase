using AutoMapper;
using MyProject.Domain.Entities;
using static MyProject.Application.Services.NguyenService;
namespace MyProject.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NguyenEntity, NguyenDto>();
        }
    }
}
