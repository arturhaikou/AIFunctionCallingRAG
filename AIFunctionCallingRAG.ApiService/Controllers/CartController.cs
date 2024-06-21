using AIFunctionCallingRAG.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIFunctionCallingRAG.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService) : ControllerBase
    {
        [HttpPost]
        public async Task<int> Create()
        {
            return await cartService.CreateAsync();
        }

        [HttpPost("{id}/items")]
        public async Task<bool> AddItems([FromRoute]int id, [FromBody]List<Guid> itemIds)
        {
            return await cartService.AddItemsAsync(itemIds, id);
        }
    }
}
