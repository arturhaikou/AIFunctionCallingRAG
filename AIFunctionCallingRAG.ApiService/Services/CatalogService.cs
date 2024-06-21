using AIFunctionCallingRAG.ApiService.Services.Interfaces;
using AIFunctionCallingRAG.Data.DbContexts;
using AIFunctionCallingRAG.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AIFunctionCallingRAG.ApiService.Services
{
    public class CatalogService(EshopDbContext dbContext) : ICatalogService
    {
        public async Task<List<Product>> GetItemsAsync()
        {
            return await dbContext.Products.ToListAsync();
        }
    }
}
