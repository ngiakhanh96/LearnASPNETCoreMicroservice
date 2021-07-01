using Npgsql;

namespace Discount.Grpc.Data
{
    public interface IDiscountContext
    {
        public NpgsqlConnection Connection { get; }
    }
}
