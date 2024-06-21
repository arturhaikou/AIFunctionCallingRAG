using AIFunctionCallingRAG.Web.Dtos;
using RestEase;

namespace AIFunctionCallingRAG.Web.ApiClients
{
    public interface IEshopApi
    {
        [Post("api/cart/{id}/items")]
        Task AddItemsToCartAsync([Path]int id, [Body] List<Guid> itemIds);

        [Post("api/cart")]
        Task<int> CreateCartAsync();

        [Post("api/orders")]
        Task<int> CreateOrderAsync([Body] int cartId);

        [Get("api/catalog/items")]
        Task<List<Product>> GetProductsAsync();

        [Get("api/orders")]
        Task<List<Order>> GetOrdersAsync();

        [Post("api/payment/buy")]
        Task<bool> BuyAsync([Body] int orderId);

        [Get("api/search")]
        Task<string> SearchAsync([Query] string name, string color);

        [Get("api/search/hybrid")]
        Task<string> HybridSearchAsync([Query] string name, string color);
    }
}
