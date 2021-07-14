using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.MappingProfiles
{
    public class DomainToResourceMappingProfile : Profile
    {
        public DomainToResourceMappingProfile()
        {
            CreateMap<Coupon, CouponDTO>();
        }
    }
}
