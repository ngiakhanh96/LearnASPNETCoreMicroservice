using AutoMapper;
using Catalog.API.DTO;
using Catalog.API.Entities;

namespace Catalog.API.MappingProfiles
{
    public class DomainToResourceMapping : Profile
    {
        public DomainToResourceMapping()
        {
            CreateMap<Product, ProductDTO>();
        }
    }
}
