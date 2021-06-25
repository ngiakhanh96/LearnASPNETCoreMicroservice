using AutoMapper;
using Basket.API.DTO;
using Basket.API.Entities;

namespace Basket.API.MappingProfiles
{
    public class ResourceToDomainMappingProfile : Profile
    {
        public ResourceToDomainMappingProfile()
        {
            CreateMap<BasketDTO, Entities.Basket>().ForMember(dest => dest.TotalPrice, act => act.Ignore());
            CreateMap<BasketItemDTO, BasketItem>();
        }
    }
}
