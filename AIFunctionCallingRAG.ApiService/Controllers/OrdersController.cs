using AIFunctionCallingRAG.ApiService.Services.Interfaces;
using AIFunctionCallingRAG.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIFunctionCallingRAG.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        [HttpPost]
        public async Task<int> Create([FromBody]int cartId)
        {
            var orderId = await orderService.CreateAsync(cartId);
            return orderId;
        }

        [HttpGet]
        public async Task<List<Order>> Get()
        {
            return await orderService.GetOrders();
        }
    }
}
