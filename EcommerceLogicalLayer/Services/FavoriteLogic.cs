using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


public class FavoriteLogic([FromKeyedServices("DataAccess")] IFavoritesRopesitry Favorites, IProductServices product) : IFavorites
{
    private readonly IFavoritesRopesitry _favorites = Favorites;
    private readonly IProductServices _product = product;

    public async Task<Result> Delete(int productID, int userID,CancellationToken cancellationToken = default)
    {
        var resul = await _product.IsExistAsync(productID);
        if(!resul)
            return Result.Fialer(new Erorr("Product Not Found", StatusCodes.Status400BadRequest));
        if (productID < 1 || userID < 1)
            return Result.Fialer(new Erorr("Invalid Data", StatusCodes.Status400BadRequest));
        var result =  await _favorites.Delete(productID, userID,cancellationToken);
        return result? Result.Seccuss() : Result.Fialer(new Erorr("Internal Server Error", StatusCodes.Status500InternalServerError));
    }

    public async Task<Result> Add(int productID, int userID, bool isFavorite = true, CancellationToken cancellationToken = default)
    {
        var resul = await _product.IsExistAsync(productID);
        if (!resul)
            return Result.Fialer(new Erorr("Product Not Found", StatusCodes.Status400BadRequest));
        if (productID < 1 || userID < 1)
            return Result.Fialer(new Erorr("Invalid Data", StatusCodes.Status400BadRequest));
        var result = await _favorites.Add(productID, userID, isFavorite);
        return result ? Result.Seccuss() : Result.Fialer(new Erorr("Internal Server Error", StatusCodes.Status400BadRequest));
    }

    public async Task<Result<List<int>>> GetByUserId(int userId, CancellationToken cancellationToken = default)
    {
        if(userId < 1)
            return Result<List<int>>.Fialer<List<int>>(new Erorr("Invalid Data", StatusCodes.Status400BadRequest));
        var result = await _favorites.GetByUserId(userId);
        return Result<List<int>>.Seccuss(result);

    }


}
