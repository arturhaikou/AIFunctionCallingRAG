using AIFunctionCallingRAG.ApiService.Services.Interfaces;
using AIFunctionCallingRAG.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIFunctionCallingRAG.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet("items")]
        public async Task<List<Product>> Get()
        {
            return await _catalogService.GetItemsAsync();
        }
    }
}
