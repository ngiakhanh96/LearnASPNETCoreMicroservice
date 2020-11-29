using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static void Seed(
            OrderContext orderContext, 
            ILoggerFactory loggerFactory,
            int currentRetry = 0)
        {
            try
            {
                orderContext.Database.Migrate();

                if (orderContext.Orders.Any())
                {
                    orderContext.Orders.AddRange(GetPreconfiguredOrders());
                    orderContext.SaveChanges();
                }
            }
            catch(Exception e)
            {
                if (currentRetry < 3)
                {
                    var log = loggerFactory.CreateLogger<OrderContextSeed>();
                    log.LogError($"{e.Message}. Current retry: {currentRetry}");
                    Seed(orderContext, loggerFactory, ++currentRetry);
                }
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    UserName = "te",
                    FirstName = "hel",
                    LastName = "or",
                    EmailAddress = "as@gmail.com",
                    AddressLine = "sasdd",
                    Country = "Turkey"
                }
            };
        }
    }
}
