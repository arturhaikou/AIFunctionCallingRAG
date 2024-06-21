using AIFunctionCallingRAG.ApiService.Services.Interfaces;
using AIFunctionCallingRAG.Data.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace AIFunctionCallingRAG.ApiService.Services
{
    public class CartService(IConnectionMultiplexer connectionMultiplexer) : ICartService
    {
        public async Task<bool> AddItemsAsync(List<Guid> ids, int cartId)
        {
            var db = connectionMultiplexer.GetDatabase();
            var isSuccess = await db.StringSetAsync(cartId.ToString(), JsonSerializer.Serialize(ids));
            return isSuccess;
        }

        public async Task<int> CreateAsync()
        {
            var db = connectionMultiplexer.GetDatabase();
            var cartId = await db.StringIncrementAsync("cartId");
            return (int)cartId;
        }

        public async Task<Cart> GetCartAsync(int id)
        {
            var db = connectionMultiplexer.GetDatabase();
            var item = await db.StringGetAsync(id.ToString());
            if (string.IsNullOrEmpty(item))
            {
                return null;
            }

            var ids = JsonSerializer.Deserialize<List<Guid>>(item);
            return new Cart { Id = id, ProdcutIds = ids };
        }
    }
}
