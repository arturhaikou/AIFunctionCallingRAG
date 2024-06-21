namespace AIFunctionCallingRAG.Web.Dtos
{
    public class OrderProduct
    {
        public int OrderId { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }
    }
}
