using AIFunctionCallingRAG.Web.ApiClients;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.ComponentModel;

namespace AIFunctionCallingRAG.Web.Services
{
    public class HyDeChatService
    {
        private readonly IEshopApi _api;
        private readonly Kernel _kernel;
        private readonly ChatHistory _history;
        private readonly OpenAIPromptExecutionSettings _aiSettings = new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };
        private int _counter = 0;

        public HyDeChatService(string key, string model, IEshopApi api, string assistantMessage)
        {
            _api = api;
            _kernel = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(model, key)
                .Build();
            _kernel.Plugins.AddFromObject(new KernelPlugin(this));
            _history = InitHistory(assistantMessage);
        }

        private ChatHistory InitHistory(string assistantMessage)
        {
            var history = new ChatHistory("""
            ## Role ##
            You are an AI customer service agent for the online retailer `Happy Shop`.


            ## Instructs ##
            Your job is to answer customer questions about products in the `Happy Shop` catalog.
            The `Happy Shop` primarily sells goods.


            ## Constraints ##
            You NEVER respond about topics other than `Happy Shop`.
            You try to be concise and only provide longer responses if necessary.
            If someone asks a question about anything other than `Happy Shop`, its catalog, or their account,
            you refuse to answer, and you instead ask if there's a topic related to `Happy Shop` you can assist with.
            You need to use products from the provided list.


            ## Examples ##
            1. Search items in the catalog
             - User: Show all bikes
             - You will call the `search_products` tool with product name `bike`.

            2. Purchase process
             - User: Buy yellow computer
             - You will call:
                1. `search_products` tool with `Bike` and `yellow` criteria
                2. `create_cart` tool
                3. `add_products_to_cart` tool
                4. `create_order` tool
                5. You must display items in the order and must ask a confirmation for purchase
                6. `purchase_order` tool.
            """);

            history.AddAssistantMessage(assistantMessage);
            return history;
        }

        public async Task<string> SendMessage(string message)
        {
            _history.AddUserMessage(message);

            var response = await _kernel.GetRequiredService<IChatCompletionService>().GetChatMessageContentAsync(_history, _aiSettings, _kernel);
            _history.Add(response);
            _counter = 0;
            return response.Content;
        }

        private class KernelPlugin(HyDeChatService chatService)
        {
            [KernelFunction("search_products"), Description("Search products by specific criteria like product name, color")]
            public async Task<string> Search(
                [Description("The name of the product from the user question. The name must start with capital letter")] string name,
                [Description("The color of the product from the user question")]  string color)
            {
                var response = await chatService._api.SearchAsync(name, color);
                return response;
            }

            [KernelFunction("create_cart"), Description("Create a cart. The tool will return the id of the created cart in the response.")]
            public async Task<string> CreateCart()
            {
                var response = await chatService._api.CreateCartAsync();
                return $"<cartId>{response}</cartId>";
            }

            [KernelFunction("add_products_to_cart"), Description("Add chosen products to the cart")]
            public async Task<string> AddItemsToCart([Description("Id of the cart")]int cartId, [Description("Ids of chosen products")] List<Guid> productIds)
            {
                await chatService._api.AddItemsToCartAsync(cartId, productIds);
                return $"Success: Yes";
            }

            [KernelFunction("create_order"), Description("Create an order from the cart")]
            public async Task<string> CreateOrder([Description("The id of the cart")] int cartId)
            {
                var orderId = await chatService._api.CreateOrderAsync(cartId);
                return $"<cartId>{orderId}</cartId>";
            }

            [KernelFunction("purchase_order"), Description("Make a purchase from the order")]
            public async Task<string> Buy([Description("The id of the order")] int orderId)
            {
                var isSuccess = await chatService._api.BuyAsync(orderId);
                var response = isSuccess ? "Yes" : "No";
                return $"Success: {response}";
            }
        }
    }
}
