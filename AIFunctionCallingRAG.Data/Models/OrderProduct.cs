namespace AIFunctionCallingRAG.Data.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }
    }
}
