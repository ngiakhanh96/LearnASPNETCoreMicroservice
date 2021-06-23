using System;

namespace Discount.API.Entities
{
    // Test with record
    public record Coupon(Guid Id)
    {
        public string ProductName { get; set; }

        public string Description { get; set; }

        public int Amount { get; set; }
    }
}
