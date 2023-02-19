using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;

namespace LE.UserService.AutoMappers
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<Language, LanguageDto>()
                .ForMember(d => d.Id, s => s.MapFrom(x => x.Langid))
                .ReverseMap();

            CreateMap<Targetlang, LanguageDto>()
                .ForMember(d => d.Id, s => s.MapFrom(x => x.Langid))
                .ForMember(d => d.Level, s => s.MapFrom(x => x.TargetLevel))
                .ReverseMap();

            CreateMap<LanguageRequest, LanguageDto>();
        }
    }
}
