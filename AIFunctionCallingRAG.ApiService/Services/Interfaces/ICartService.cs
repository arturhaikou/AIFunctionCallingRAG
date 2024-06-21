using AIFunctionCallingRAG.Data.Models;

namespace AIFunctionCallingRAG.ApiService.Services.Interfaces
{
    public interface ICartService
    {
        Task<bool> AddItemsAsync(List<Guid> ids, int cartId);
        Task<Cart> GetCartAsync(int id);
        Task<int> CreateAsync();
    }
}
