using Npgsql;

namespace Discount.API.Data
{
    public interface IDiscountContext
    {
        public NpgsqlConnection Connection { get; }
    }
}
