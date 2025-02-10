

namespace EcommerceDataLayer.IRopesitry
{
    public interface  IFavoritesRopesitry
    {

        public Task<bool> Delete(int productID, string userID,CancellationToken cancellationToken = default);

        public Task<bool> Add(int productID, string userID, bool isFavorite = true, CancellationToken cancellationToken = default);

        public Task<List<int>> GetByUserId(string userId, CancellationToken cancellationToken = default);
    }
}
