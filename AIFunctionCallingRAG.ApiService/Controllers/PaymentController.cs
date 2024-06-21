using AIFunctionCallingRAG.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIFunctionCallingRAG.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        [HttpPost("buy")]
        public async Task<bool> Buy([FromBody]int orderId)
        {
            return await paymentService.BuyAsync(orderId);
        }
    }
}
