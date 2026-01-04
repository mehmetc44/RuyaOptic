namespace RuyaOptik.Business.Consts
{
    public static class CacheKeys
    {
        // VERSION KEYS (global invalidation)
        public const string Product_Version = "products:version";
        public const string Category_Version = "categories:version";

        // CATEGORY
        public static string Categories_All(int version)
            => $"categories:all:v{version}";

        // PRODUCT
        public static string Product_ById(int id)
            => $"products:id:{id}";

        public static string Products_List(int version, string key)
            => $"products:list:v{version}:{key}";

        // INVENTORY
        public static string Inventory_ByProduct(int productId)
            => $"inventory:product:{productId}";
    }
}
