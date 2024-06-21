using AIFunctionCallingRAG.Data.Models;

namespace AIFunctionCallingRAG.ApiService.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateAsync(int cartId);
        Task<List<Order>> GetOrders();
        Task<Order> GetOrderByIdAsync(int id);
    }
}
