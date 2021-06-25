using AutoMapper;
using Catalog.API.DTO;
using Catalog.API.Entities;

namespace Catalog.API.MappingProfiles
{
    public class ResourceToDomainMappingProfile : Profile
    {
        public ResourceToDomainMappingProfile()
        {
            CreateMap<ProductDTO, Product>();
        }
    }
}
