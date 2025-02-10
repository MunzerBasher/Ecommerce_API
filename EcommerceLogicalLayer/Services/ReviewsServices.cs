using EcommerceDataLayer.AppDbContex;
using EcommerceDataLayer.Entities.Reviews;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLogicalLayer.Services
{
    public class ReviewsServices(IReviewsRopesitry reviewsRopesitry,
        ApplicationDbContext applicationDbContext,
        IProductServices productServices ) : IReviewsServices
    {
        private readonly IReviewsRopesitry _reviewsRopesitry = reviewsRopesitry;
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        private readonly IProductServices _productServices = productServices;


        public async Task<Result<bool>> AddAsync(ReviewRequest review)
        {
            if (!await _applicationDbContext.Users.AnyAsync(x => x.Id == review.UserId))
                return Result<bool>.Failure<bool>(new Error(UserErrors.NotFound, StatusCodes.Status400BadRequest));
            if (!await _productServices.IsExistAsync(review.ProductId))
                return Result<bool>.Failure<bool>(new Error(ProductsError.ImageNotFound, StatusCodes.Status404NotFound));
            var result = await _reviewsRopesitry.AddAsync(review);
            return result ? Result<bool>.Seccuss(result) : Result<bool>.Failure<bool>(new Error(ReviewsErrors.ServerError, StatusCodes.Status500InternalServerError));
        
        }

        public async Task<Result<bool>> DeleteAsync(int ReviewId)
        {
            if(! await _reviewsRopesitry.IsExistAsync(ReviewId))
                return Result<bool>.Failure<bool>(new Error(ReviewsErrors.NotFound, StatusCodes.Status404NotFound));
            var result = await _reviewsRopesitry.DeleteAsync(ReviewId);
            return result ? Result<bool>.Seccuss(result) : Result<bool>.Failure<bool>(new Error(ReviewsErrors.ServerError, StatusCodes.Status500InternalServerError));

        }

        public async Task<Result<List<ReviewResponse>>> GetAllProductReviewsAsync(int ProductId)
        {
            if(! await _productServices.IsExistAsync(ProductId))
                return Result<List<ReviewResponse>>.Failure<List<ReviewResponse>>(new Error(ProductsError.ImageNotFound, StatusCodes.Status404NotFound));          
            var result = await _reviewsRopesitry.GetAllProductReviewsAsync(ProductId);
            return Result<List<ReviewResponse>>.Seccuss(result);

        }

        public async Task<Result<List<ReviewResponse>>> GetAllUserReviewsInProductAsync(string UserId, int ProductId)
        {
            if (!await _applicationDbContext.Users.AnyAsync(x => x.Id == UserId))
                return Result<List<ReviewResponse>>.Failure<List<ReviewResponse>>(new Error(UserErrors.NotFound, StatusCodes.Status404NotFound));
            var result = await _reviewsRopesitry.GetAllUserInProductReviewsAsync(UserId,ProductId);
            return Result<List<ReviewResponse>>.Seccuss(result);

        }

        public async Task<Result<List<ReviewResponse>>> GetAllUserReviewsAsync(string UserId)
        {
            if (!await _applicationDbContext.Users.AnyAsync(x => x.Id == UserId))
                return Result<List<ReviewResponse>>.Failure<List<ReviewResponse>>(new Error(UserErrors.NotFound, StatusCodes.Status404NotFound));
            var result = await _reviewsRopesitry.GetAllUserReviewsAsync(UserId);
            return Result<List<ReviewResponse>>.Seccuss(result);
        }

        public Task<bool> IsExistAsync(int ReviewId) => _reviewsRopesitry.IsExistAsync(ReviewId);

        public async Task<Result<bool>> UpdateAsync(UpdateReviewRequest updateReviewRequest, int ReviewId)
        {
            if(! await IsExistAsync(ReviewId))
                return Result<bool>.Failure<bool>(new Error(ReviewsErrors.NotFound, StatusCodes.Status404NotFound));
            var result = await _reviewsRopesitry.UpdateAsync(updateReviewRequest, ReviewId);
            return result ? Result<bool>.Seccuss(result) :
                Result<bool>.Failure<bool>(new Error(ReviewsErrors.ServerError, StatusCodes.Status500InternalServerError));

        }
    }


}