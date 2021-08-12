using Basket.API.DTO;
using AutoMapper;
using EventBus.Messages.Events;

namespace Basket.API.MappingProfiles
{
    public class ResourceToEventMappingProfile : Profile
    {
        public ResourceToEventMappingProfile()
        {
            CreateMap<BasketCheckoutDTO, BasketCheckoutEvent>();
        }
    }
}
