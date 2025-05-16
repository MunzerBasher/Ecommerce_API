

namespace EcommerceDataLayer.IRopesitry
{
    public interface  IFavoritesRopesitry
    {

        public Task<bool> Delete(int productID, int userID,CancellationToken cancellationToken = default);

        public Task<bool> Add(int productID, int userID, bool isFavorite = true, CancellationToken cancellationToken = default);

        public Task<List<int>> GetByUserId(int userId, CancellationToken cancellationToken = default);
    }
}
