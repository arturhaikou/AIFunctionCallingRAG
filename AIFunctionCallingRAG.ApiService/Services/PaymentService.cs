using AIFunctionCallingRAG.ApiService.Services.Interfaces;
using AIFunctionCallingRAG.Data.DbContexts;
using AIFunctionCallingRAG.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AIFunctionCallingRAG.ApiService.Services
{
    public class PaymentService(EshopDbContext dbContext) : IPaymentService
    {
        public async Task<bool> BuyAsync(int orderId)
        {
            var order = await dbContext.Orders.FirstOrDefaultAsync(order => order.Id == orderId);

            if (order is null)
            {
                throw new ArgumentException("It's not possible to find an order");
            }

            order.Status = OrderStatus.Paid;
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
