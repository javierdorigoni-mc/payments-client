namespace PaymentsClient.Domain.Payments;

public interface IPaymentsService
{    
    Task<Result<CreatePaymentResponse>> CreatePaymentAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default);
    Task<Result<RefreshPaymentStatusResponse>> RefreshStatusAsync(RefreshPaymentStatusRequest request, CancellationToken cancellationToken = default);
}