using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = new Coupon()
            {
                Id = request.Coupon.Id,
                ProductName = request.Coupon.ProducName,
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description,
            };

            if (request.Coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon"));
            }

            logger.LogInformation("Discount is succesfully created. Product name is {productName}", coupon.ProductName);

            await dbContext.Coupons.AddAsync(coupon);
            await dbContext.SaveChangesAsync();


            return new CouponModel()
            {
                Id = coupon.Id,
                Amount = coupon.Amount,
                Description = coupon.Description,
                ProducName = coupon.ProductName
            };
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName.Equals(request.ProductName));

            if (coupon is null)
            {
                coupon = new Coupon()
                {
                    Id = 0,
                    ProductName = "No discount",
                    Amount = 0,
                    Description = "No discount"
                };
            }
            logger.LogInformation("Discount is retrived for ProductName: {productName}", coupon.ProductName);
            var couponModel = new CouponModel()
            {
                Id = coupon.Id,
                ProducName = coupon.ProductName,
                Amount = coupon.Amount,
                Description = coupon.Description,
            };
            return couponModel;

        }

        public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == request.Coupon.Id);

            if (request.Coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon"));
            }

            coupon.Description = request.Coupon.Description ?? coupon.Description;
            coupon.Amount = request.Coupon.Amount > 0 ? request.Coupon.Amount : coupon.Amount;
            coupon.ProductName = request.Coupon.ProducName ?? coupon.ProductName;

            logger.LogInformation("Discount is succesfully updated. Product name is {productName}", coupon.ProductName);

            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();


            return new CouponModel()
            {
                Id = coupon.Id,
                Amount = coupon.Amount,
                Description = coupon.Description,
                ProducName = coupon.ProductName
            };
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName.Equals(request.ProducName));
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
            }
            logger.LogInformation("Discount is succesfully deleted. Product name is {productName}", coupon.ProductName);

            dbContext.Remove(coupon);
            await dbContext.SaveChangesAsync();
            return new DeleteDiscountResponse() { Success = true };
        }
    }
}
