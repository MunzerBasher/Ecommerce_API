

using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{
    public interface  IFavorites
    {

        public Task<Result> Delete(int productID, string userID,CancellationToken cancellationToken = default);

        public Task<Result> Add(int productID, string userID, bool isFavorite = true, CancellationToken cancellationToken = default);

        public Task<Result<List<int>>> GetByUserId(string userId, CancellationToken cancellationToken = default);
    }
}
