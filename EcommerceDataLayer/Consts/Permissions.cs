namespace SurveyManagementSystemApi.Abstractions.Consts
{


    public static class Permissions
    {
        public static string Type { get; } = "permissions";

        public const string GetProfile = "Profile:read";
        public const string UpdateProfile = "Profile:update";
        public const string ChangePassword = "Profile:Change-Password";

        public const string GetFavorites = "Favorites:read";
        public const string AddFavorites = "Favorites:add";
        public const string DeleteFavorites = "Favorites:Deletet";

        public const string AddItems = "Items:add";
        public const string GetItems = "Items:read";
        public const string UpdateItems = "Items:update";
        public const string DeleteItems = "Items:deletet";

        public const string AddOrders = "Orders:add";
        public const string GetOrders = "Orders:read";
        public const string UpdateOrders = "Orders:update";
        public const string DeleteOrders = "Orders:deletet";


        public const string GetReviews = "Reviews:read";
        public const string UpdateReviews = "Reviews:update";
        public const string DeletetReviews = "Reviews:deletet";



        public const string GetAddress = "Address:read";
        public const string AddAddress = "Address:add";
        public const string UpdateAddress = "Address:update";
        public const string DeleteAddress = "Address:deletet";

        public const string GetProductsImage = "Products-Image:read";
        public const string DeleteProductsImage = "Products-Image:deletet";
        public const string AddProductsImage = "Products-Image:add";
        public const string AddProducts = "Products:add";
        public const string GetProducts = "Products:read";
        public const string UpdateProducts = "Products:update";
        public const string DeleteProducts = "Products:deletet";

        public const string AddShippings = "Shippings:add";
        public const string GetShippings = "Shippings:read";
        public const string UpdateShippings = "Shippings:update";
        public const string DeleteShippings = "Shippings:deletet";

        public const string DeleteCategories = "Shippings:deletet";
        public const string GetCategories = "Categories:read";
        public const string AddCategories = "Categories:add";
        public const string UpdateCategories = "Categories:update";


        public const string UploadImage = "Upload-Image:add";


        public const string GetUsers = "users:read";
        public const string AddUsers = "users:add";
        public const string UpdateUsers = "users:update";

        public const string GetRoles = "roles:read";
        public const string AddRoles = "roles:add";
        public const string UpdateRoles = "roles:update";

        public const string GetResult = "results:read";

        public static IList<string?> GetAllPermissions() =>
            typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList();
    }
}