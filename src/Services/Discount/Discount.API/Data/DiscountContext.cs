using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.API.Data
{
    public class DiscountContext : IDiscountContext
    {
        private readonly IConfiguration _configuration;

        public NpgsqlConnection Connection { get; }

        public DiscountContext(IConfiguration configuration)
        {
            _configuration = configuration;
            Connection = new NpgsqlConnection(_configuration.GetSection("DatabaseSettings:ConnectionString").Value);
        }
    }
}
