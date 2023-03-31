using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LE.UserService.AutoMappers
{
    public class VocabularyProfile : Profile
    {
        public VocabularyProfile()
        {
            CreateMap<VocabularyRequest, VocabularyPackageDto>()
                .ForMember(d => d.VocabularyDtos, s => s.MapFrom(x => x.vocabularyPairs));

            CreateMap<VocabularyPair, VocabularyDto>();

            CreateMap<VocabularyPackageDto, Vocabpackage>()
                .ForMember(d => d.Name, s => s.MapFrom(x => x.Title))
                .ForMember(d => d.VocabularyPairs, s => s.MapFrom(x => JsonConvert.SerializeObject(x.VocabularyDtos)));

            CreateMap<Vocabpackage, VocabularyPackageDto>()
                .ForMember(d => d.Title, s => s.MapFrom(x => x.Name))
                .ForMember(d => d.VocabularyDtos, s => s.MapFrom(x => JsonConvert.DeserializeObject<List<VocabularyDto>>(x.VocabularyPairs)));
        }
    }
}
