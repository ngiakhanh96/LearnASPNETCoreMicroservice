using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static void SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                orderContext.SaveChanges();
                logger.LogInformation("Seed database with context {DbContextName}", nameof(OrderContext));
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new() {UserName = "WhiteK", FirstName = "Khan", LastName = "Nguyen", EmailAddress = "abc@gmail.com"}
            };
        }
    }
}
