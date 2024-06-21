namespace AIFunctionCallingRAG.ApiService.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> BuyAsync(int orderId);
    }
}
