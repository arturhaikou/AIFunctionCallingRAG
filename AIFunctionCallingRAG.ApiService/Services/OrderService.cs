using AIFunctionCallingRAG.ApiService.Services.Interfaces;
using AIFunctionCallingRAG.Data.DbContexts;
using AIFunctionCallingRAG.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AIFunctionCallingRAG.ApiService.Services
{
    public class OrderService(EshopDbContext dbContext, ICartService cartService) : IOrderService
    {
        public async Task<int> CreateAsync(int cartId)
        {
            var cart = await cartService.GetCartAsync(cartId);

            if (cart == null)
            {
                throw new ArgumentNullException("It's not possible to find a cart");
            }

            var order = await CreateOrder();
            await CreateOrderProductsAsync(order, cart);
            return order.Id;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await dbContext.Orders.FirstOrDefaultAsync(order => order.Id == id);
        }

        public async Task<List<Order>> GetOrders()
        {
            return await dbContext.Orders
                .Include(order => order.Products)
                .ThenInclude(product => product.Product)
                .ToListAsync();
        }

        private async Task<Order> CreateOrder()
        {
            var order = new Order
            {
                Status = OrderStatus.Created,
                CreateDate = DateTime.UtcNow,
            };

            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            return order;
        }

        private async Task CreateOrderProductsAsync(Order order, Cart cart)
        {
            var orderProducts = cart.ProdcutIds.Select(productId => new OrderProduct { OrderId = order.Id, ProductId = productId });
            dbContext.OrderProducts.AddRange(orderProducts);
            await dbContext.SaveChangesAsync();
        }
    }
}
