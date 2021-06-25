using AutoMapper;
using Discount.API.DTO;
using Discount.API.Entities;

namespace Discount.API.MappingProfiles
{
    public class DomainToResourceMappingProfile : Profile
    {
        public DomainToResourceMappingProfile()
        {
            CreateMap<Coupon, CouponDTO>();
        }
    }
}
