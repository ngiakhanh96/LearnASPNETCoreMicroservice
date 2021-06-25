using AutoMapper;
using Basket.API.DTO;
using Basket.API.Entities;

namespace Basket.API.MappingProfiles
{
    public class DomainToResourceMappingProfile : Profile
    {
        public DomainToResourceMappingProfile()
        {
            CreateMap<Entities.Basket, BasketDTO>();
            CreateMap<BasketItem, BasketItemDTO>();
        }
    }
}
