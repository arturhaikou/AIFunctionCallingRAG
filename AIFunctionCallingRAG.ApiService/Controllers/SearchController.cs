using AIFunctionCallingRAG.ApiService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIFunctionCallingRAG.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet("")]
        public async Task<string> Search([FromKeyedServices("dense-search")] SearchService service, [FromQuery] string name, [FromQuery] string? color)
        {
            return await service.SearchAsync(name, color);
        }

        [HttpGet("hybrid")]
        public async Task<string> HybridSearch([FromKeyedServices("hybrid-search")] SearchService service, [FromQuery] string name, [FromQuery] string? color)
        {
            return await service.SearchAsync(name, color);
        }
    }
}
