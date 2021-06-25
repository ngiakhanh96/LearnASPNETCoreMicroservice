using System;

namespace Discount.API.Entities
{
    // Test with record
    public record Coupon(
        Guid Id, 
        string ProductName,
        string Description,
        int Amount);
}
