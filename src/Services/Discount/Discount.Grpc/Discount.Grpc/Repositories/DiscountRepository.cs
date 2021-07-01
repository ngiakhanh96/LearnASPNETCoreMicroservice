using System;
using System.Threading.Tasks;
using Dapper;
using Discount.Grpc.Data;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly NpgsqlConnection _connection;

        public DiscountRepository(IDiscountContext discountContext)
        {
            _connection = discountContext.Connection;
        }

        public Task<Coupon> GetCoupon(Guid id)
        {
            return _connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE Id = @Id",
                new
                {
                    Id = id
                });
        }

        public async Task<Guid> CreateCoupon(Coupon coupon)
        {
            var insertedId = await _connection.ExecuteScalarAsync<Guid>(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount) RETURNING Id",
                coupon);

            return insertedId;
        }

        public async Task<bool> UpdateCoupon(Coupon coupon)
        {
            var affected = await _connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id = @Id",
                coupon);

            return affected > 0;
        }

        public async Task<bool> DeleteCoupon(Guid id)
        {
            var affected = await _connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE Id = @Id",
                new
                {
                    Id = id
                });

            return affected > 0;
        }
    }
}
