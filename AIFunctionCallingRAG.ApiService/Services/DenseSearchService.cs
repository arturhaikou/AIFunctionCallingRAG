using AIFunctionCallingRAG.Data.Models;
using Azure.AI.OpenAI;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using static Qdrant.Client.Grpc.Conditions;

namespace AIFunctionCallingRAG.ApiService.Services
{
    public class DenseSearchService(OpenAIClient openAIClient, QdrantClient qdrantClient) : SearchService(openAIClient, "products")
    {
        public override async Task<string> SearchAsync(string productName, string color)
        {
            var embeddings = await GetEmbeddingsAsync(productName, color);
            var response = await SearchByVectorAsync(embeddings, productName, color);
            return response;
        }

        private async Task<string> SearchByVectorAsync(float[] vector, string productName, string color)
        {
            var scorePoints = await qdrantClient.SearchAsync(CollectionName, vector, filter: GetFilter(productName, color));
            return CreateResponse(scorePoints);
        }

        private Filter GetFilter(string productName, string color)
        {
            var filter = new Filter()
            {
                Should =
                {
                     MatchText(nameof(Product.Name), productName),
                     MatchText(nameof(Product.Description), productName)
                }
            };

            if (color is not null)
            {
                filter.MinShould = new MinShould
                {
                    Conditions =
                    {
                        MatchText(nameof(Product.Color), color)
                    }
                };
            }

            return filter;
        }
    }
}
