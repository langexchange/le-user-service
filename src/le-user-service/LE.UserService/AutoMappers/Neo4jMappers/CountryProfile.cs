using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Neo4jData.DALs.Schemas;

namespace LE.UserService.AutoMappers.Neo4jMappers
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<CountrySchema, CountryDto>();
        }
    }
}
