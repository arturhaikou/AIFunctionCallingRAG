﻿using AIFunctionCallingRAG.Web.ApiClients;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json;
using System.Text;

namespace AIFunctionCallingRAG.Web.Services
{
    public class ChatService
    {
        private readonly IEshopApi _api;
        private readonly Kernel _kernel;
        private readonly ChatHistory _history;
        private readonly OpenAIPromptExecutionSettings _aiSettings = new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };

        public ChatService(string key, string model, IEshopApi api, string assistantMessage)
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
            
            
            ## Examples ##
            1. Search items in the catalog
             - User: Show all bikes
             - You will call the `search_products` tool and return response based on the tool output.
            
            2. Purchase process
             - User: Buy yellow computer
             - You will call:
                1. `search_products` tool
                2. `create_cart` tool
                3. `add_products_to_cart` tool
                4. `create_order` tool
                5. You must display items in the order and must ask a confirmation for purchase
                6. `purchase_order` tools in sequence.
            """);

            history.AddAssistantMessage(assistantMessage);
            return history;
        }

        public async Task<string> SendMessage(string message)
        {
            _history.AddUserMessage(message);

            var response = await _kernel.GetRequiredService<IChatCompletionService>().GetChatMessageContentAsync(_history, _aiSettings, _kernel);
            _history.Add(response);
            return response.Content;
        }

        private class KernelPlugin(ChatService chatService)
        {
            [KernelFunction("get_products"), Description("Get all products from the catalog")]
            public async Task<string> GetProducts()
            {
                var response = await chatService._api.GetProductsAsync();
                var builder = new StringBuilder("id,name,description,color,size,price");
                foreach (var product in response)
                {
                    builder.AppendLine($"{product.Id},{product.Name},{product.Description},{product.Color},{product.Size},{product.Price}");
                    builder.AppendLine();
                }

                return builder.ToString();
            }

            [KernelFunction("create_cart"), Description("Create a cart")]
            public async Task<string> CreateCart()
            {
                var response = await chatService._api.CreateCartAsync();
                return $"CartId: {response}";
            }

            [KernelFunction("add_products_to_cart"), Description("Add chosen products to the cart")]
            public async Task<string> AddItemsToCart([Description("Id of the cart")] int cartId, [Description("Ids of chosen products")] List<Guid> productIds)
            {
                await chatService._api.AddItemsToCartAsync(cartId, productIds);
                return $"Success: Yes";
            }

            [KernelFunction("create_order"), Description("Create an order from the cart")]
            public async Task<string> CreateOrder([Description("The id of the cart")] int cartId)
            {
                var orderId = await chatService._api.CreateOrderAsync(cartId);
                return $"OrderId: {orderId}";
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
