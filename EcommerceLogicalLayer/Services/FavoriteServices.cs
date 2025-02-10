using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


public class FavoriteServices( IFavoritesRopesitry Favorites, IProductServices product) : IFavorites
{
    private readonly IFavoritesRopesitry _favorites = Favorites;
    private readonly IProductServices _product = product;

    public async Task<Result> Delete(int productID, string userID,CancellationToken cancellationToken = default)
    {
        var resul = await _product.IsExistAsync(productID);
        if(!resul)
            return Result.Failure(new Error("Product Not Found", StatusCodes.Status400BadRequest));
        if (productID < 1 || userID is null)
            return Result.Failure(new Error("Invalid Data", StatusCodes.Status400BadRequest));
        var result =  await _favorites.Delete(productID, userID!,cancellationToken);
        return result? Result.Seccuss() : Result.Failure(new Error("Internal Server Error", StatusCodes.Status500InternalServerError));
    }

    public async Task<Result> Add(int productID, string userID, bool isFavorite = true, CancellationToken cancellationToken = default)
    {
        var resul = await _product.IsExistAsync(productID);
        if (!resul)
            return Result.Failure(new Error("Product Not Found", StatusCodes.Status400BadRequest));
        if (productID < 1 || userID is null)
            return Result.Failure(new Error("Invalid Data", StatusCodes.Status400BadRequest));
        var result = await _favorites.Add(productID, userID!, isFavorite);
        return result ? Result.Seccuss() : Result.Failure(new Error("Internal Server Error", StatusCodes.Status400BadRequest));
    }

    public async Task<Result<List<int>>> GetByUserId(string userId, CancellationToken cancellationToken = default)
    {
        if(userId is null)
            return Result<List<int>>.Failure<List<int>>(new Error("Invalid Data", StatusCodes.Status400BadRequest));
        var result = await _favorites.GetByUserId(userId);
        return Result<List<int>>.Seccuss(result);

    }


}
