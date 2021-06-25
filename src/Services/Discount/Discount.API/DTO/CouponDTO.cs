using System;

namespace Discount.API.DTO
{
    public record CouponDTO(
        Guid Id,
        string ProductName,
        string Description,
        int Amount);
}
