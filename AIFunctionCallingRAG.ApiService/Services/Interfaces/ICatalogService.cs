using AIFunctionCallingRAG.Data.Models;

namespace AIFunctionCallingRAG.ApiService.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<Product>> GetItemsAsync();
    }
}
