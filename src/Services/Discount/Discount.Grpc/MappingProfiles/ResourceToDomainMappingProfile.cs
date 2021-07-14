using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.MappingProfiles
{
    public class ResourceToDomainMappingProfile : Profile
    {
        public ResourceToDomainMappingProfile()
        {
            CreateMap<CouponDTO, Coupon>();
        }
    }
}
