using Parser.Common.SqlManager.Models;
using Parser.Service.Models.Dtos;
using AutoMapper;
using Attribute = Parser.Common.SqlManager.Models.Attribute;

namespace Parser.Service.MapperConfig
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Attribute, AttributesDto>();
            CreateMap<Brand, BrandDto>();
            CreateMap<Complectation, ComplectationDto>();
            CreateMap<Detail, DetailDto>();
            CreateMap<Group, GroupDto>();
            CreateMap<Model, ModelDto>();
            CreateMap<Schema, SchemaDto>();
            CreateMap<SubGroup, SubGroupDto>();
            CreateMap<Global, GlobalDto>();
        }
    }
}
