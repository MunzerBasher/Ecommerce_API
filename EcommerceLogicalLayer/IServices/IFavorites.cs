

using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{
    public interface  IFavorites
    {

        public Task<Result> Delete(int productID, int userID,CancellationToken cancellationToken = default);

        public Task<Result> Add(int productID, int userID, bool isFavorite = true, CancellationToken cancellationToken = default);

        public Task<Result<List<int>>> GetByUserId(int userId, CancellationToken cancellationToken = default);
    }
}
