using RestEase;

namespace AIFunctionCallingRAG.ApiService.Apis
{
    public interface ISparseVectorGeneratorApi
    {
        [Post("/")]
        Task<SparseVectorResponse> GetSparseVectorAsync([Body] SparseVectorRequest request);
    }

    public class SparseVectorRequest
    {
        public string text { get; set; }
    }

    public class SparseVectorResponse
    {
        public uint[] Indices { get; set; }

        public float[] Values { get; set; }
    }
}
