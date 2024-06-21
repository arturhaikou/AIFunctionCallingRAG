namespace AIFunctionCallingRAG.Web.Dtos
{
    public class Order
    {
        public int Id { get; set; }

        public ICollection<OrderProduct> Products { get; set; } = new List<OrderProduct>();

        public DateTime CreateDate { get; set; }

        public OrderStatus Status { get; set; }
    }
}
