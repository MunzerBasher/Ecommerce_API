public class FavoriteLogic
{
    
    public static void AddProductToFavorite(int productID, int userID, bool isFavorite)
    {
        // Add any business logic here before calling the DataLayer
        if (productID <= 0 || userID <= 0)
        {
            throw new ArgumentException("Invalid product ID or user ID");
        }

        FavoriteDataAccess.AddToFavorite(productID, userID, isFavorite);
    }

    public static void RemoveProductFromFavorite(int productID, int userID)
    {
        // Add any business logic here before calling the DataLayer
        if (productID <= 0 || userID <= 0)
        {
            throw new ArgumentException("Invalid product ID or user ID");
        }

        FavoriteDataAccess.DeleteFromFavorite(productID, userID);
    }

    public static List<int> GetAllFavoriteProductsByUserID(int userID)
    {
        return FavoriteDataAccess.GetFavoritesByUserId(userID);
    }

}
