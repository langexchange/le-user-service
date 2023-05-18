using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;
using LE.UserService.Models.Responses;
using LE.UserService.Neo4jData.DALs.Schemas;

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
            CreateMap<LanguageDto, LangResponse>();

            //Neo4j
            CreateMap<LangSchema, Neo4jLangDto>();
            CreateMap<LevelLangScheme, LevelNeo4jLangDto>();

            CreateMap<Language, Neo4jLangDto>()
                .ForMember(d => d.Id, s => s.MapFrom(x => x.Langid));
        }
    }
}
