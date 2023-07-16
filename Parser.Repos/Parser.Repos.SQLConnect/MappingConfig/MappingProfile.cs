using Parser.Common.SqlManager.Models;
using Parser.Common.Shared;
using Parser.Repos.SQLConnect.Entity;
using AutoMapper;
using Attribute = Parser.Common.SqlManager.Models.Attribute;

namespace Parser.Repos.SQLConnect.MappingConfig
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ItemIdEntity, ItemId>();
            CreateMap<BrandEntity, Brand>();
            CreateMap<ModelEntity, Model>();
            CreateMap<ComplectationEntity, Complectation>();
            CreateMap<AttributeEntity, Attribute>();
            CreateMap<GroupEntity, Group>();
            CreateMap<SubGroupEntity, SubGroup>();
            CreateMap<DetailEntity, Detail>();
            CreateMap<SchemaEntity, Schema>()
                .ForMember(opts => opts.ImageUrl, option => option.MapFrom<UrlResolver<SchemaEntity, Schema>, string>(r => r.ImageName));
            CreateMap<GlobalEntity, Global>()
                .ForMember(opts => opts.ImageUrl, option => option.MapFrom<UrlResolver<GlobalEntity, Global>, string>(r => r.ImageName));
            CreateMap<TableResponse, TableInfo>();
        }
    }
}
