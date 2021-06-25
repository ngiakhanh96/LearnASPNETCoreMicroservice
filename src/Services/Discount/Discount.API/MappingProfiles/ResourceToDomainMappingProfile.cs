using AutoMapper;
using Discount.API.DTO;
using Discount.API.Entities;

namespace Discount.API.MappingProfiles
{
    public class ResourceToDomainMappingProfile : Profile
    {
        public ResourceToDomainMappingProfile()
        {
            CreateMap<CouponDTO, Coupon>();
        }
    }
}
