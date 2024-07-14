using AIFunctionCallingRAG.ApiService.Apis;
using Azure.AI.OpenAI;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace AIFunctionCallingRAG.ApiService.Services
{
    public class HybridSearchService(OpenAIClient openAIClient, QdrantClient qdrantClient, ISparseVectorGeneratorApi api) : SearchService(openAIClient, "products-sparse")
    {
        public override async Task<string> SearchAsync(string productName, string color)
        {
            var embeddings = await GetEmbeddingsAsync(productName, color);
            var sparse = await GetSparseValuesAsync($"{color} {productName}");
            var searchPoints = GetBatchSearchPoints(embeddings, sparse);
            var batchSearchResults = await qdrantClient.SearchBatchAsync(CollectionName, searchPoints);
            var scoredPoints = RRFMixing(batchSearchResults);
            var response = CreateResponse(scoredPoints);
            return response;
        }

        private List<SearchPoints> GetBatchSearchPoints(float[] embeddings, (float[] Values, uint[] Indices) sparse)
        {
            return new List<SearchPoints>
            {
                new SearchPoints
                {
                    VectorName = "dense",
                    Vector = { embeddings },
                    Limit = 5,
                    WithPayload = true
                },
                new SearchPoints
                {
                    VectorName  = "sparse",
                    Vector = { sparse.Values },
                    SparseIndices = new SparseIndices
                    {
                        Data = { sparse.Indices }
                    },
                    Limit = 5,
                    WithPayload = true
                }
            };
        }

        private async Task<(float[] Values, uint[] Indices)> GetSparseValuesAsync(string input)
        {
            var response = await api.GetSparseVectorAsync(new SparseVectorRequest { text = input });
            return (response.Values, response.Indices);
        }

        private List<ScoredPoint> RRFMixing(IReadOnlyList<BatchResult> results)
        {
            Dictionary<string,(ScoredPoint ScoredPoint, double Rank)> ranks = new Dictionary<string, (ScoredPoint ScoredPoint, double Rank)>();
            foreach (var batchResult in results)
            {
                _ = batchResult.Result.Select((scoredPoint, index) =>
                {
                    if (ranks.TryGetValue(scoredPoint.Id.Uuid, out (ScoredPoint, double Rank) value))
                    {
                        value.Rank += 1 / (index + 1);
                        ranks[scoredPoint.Id.Uuid] = value;
                    }
                    else
                    {
                        ranks.Add(scoredPoint.Id.Uuid, (scoredPoint, 1 / (index + 1)));
                    }

                    return scoredPoint;
                }).ToList();
            }

            return ranks.OrderByDescending(x => x.Value.Rank).Select(x => x.Value.ScoredPoint).ToList();
        }
    }
}
