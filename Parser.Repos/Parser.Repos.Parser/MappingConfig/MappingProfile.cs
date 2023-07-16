using Parser.Common.Parser.ParsedPages;
using Parser.Repos.Parser.Models;
using AutoMapper;

namespace Parser.Repos.Parser.MappingConfig
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CarEntity, Car>();
            CreateMap<ComplectationInfoEntity, ComplectationInfo>();
            CreateMap<DetailEntity, Detail>();
            CreateMap<CarPageEntity, CarPage>();
            CreateMap<ComplectationPageEntity, ComplectationPage>();
            CreateMap<SpareGroupPageEntity, GroupPage>();
            CreateMap<SpareSubGroupPageEntity, SubGroupPage>();
            CreateMap<SpareDetailPageEntity, DetailPage>();
        }
    }
}
