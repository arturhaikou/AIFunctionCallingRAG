using AIFunctionCallingRAG.Data.Models;
using Azure.AI.OpenAI;
using Qdrant.Client.Grpc;
using System.Text;
using System.Text.Json;

namespace AIFunctionCallingRAG.ApiService.Services
{
    public abstract class SearchService(OpenAIClient openAIClient, string collectionName)
    {
        public string CollectionName { get; } = collectionName;

        public abstract Task<string> SearchAsync(string productName, string color);

        protected async Task<float[]> GetEmbeddingsAsync(string productName, string color)
        {
            var productAsString = GetHypotheticalDocument(null, productName, productName, color, null, null);
            var response = await openAIClient.GetEmbeddingsAsync(GetEmbeddingsOptions(productAsString));
            return response.Value.Data.First().Embedding.ToArray();
        }

        private EmbeddingsOptions GetEmbeddingsOptions(string input)
        {
            return new EmbeddingsOptions
            {
                DeploymentName = "text-embedding-ada-002",
                Input = { input }
            };
        }

        protected virtual string GetHypotheticalDocument(string id, string name, string description, string color, string size, string price)
        {
            return $"""
                Id: {id}
                Name: {name}
                Description: {description}
                Color: {color}
                Size: {size}
                Price: {price}
                """;
        }

        protected string CreateResponse(IReadOnlyList<ScoredPoint> scorePoints)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("id,name,description,color,size,price");
            foreach (var item in scorePoints)
            {
                //var hypotheticalDocument = GetHypotheticalDocument(
                //    item.Payload[nameof(Product.Id)].StringValue,
                //    item.Payload[nameof(Product.Name)].StringValue,
                //    item.Payload[nameof(Product.Description)].StringValue,
                //    item.Payload[nameof(Product.Color)].StringValue,
                //    item.Payload[nameof(Product.Size)].StringValue,
                //    item.Payload[nameof(Product.Price)].StringValue);
                //stringBuilder.Append(hypotheticalDocument);
                //stringBuilder.AppendLine();
                //stringBuilder.AppendLine();

                //var asString = JsonSerializer.Serialize(item);
                var id = item.Payload[nameof(Product.Id)].StringValue;
                var name = item.Payload[nameof(Product.Name)].StringValue;
                var description = item.Payload[nameof(Product.Description)].StringValue;
                var color = item.Payload[nameof(Product.Color)].StringValue;
                var size = item.Payload[nameof(Product.Size)].StringValue;
                var price = item.Payload[nameof(Product.Price)].StringValue;

                stringBuilder.AppendLine($"{id},{name},{description},{color},{size},{price}");
            }

            return stringBuilder.ToString();
        }
    }
}
