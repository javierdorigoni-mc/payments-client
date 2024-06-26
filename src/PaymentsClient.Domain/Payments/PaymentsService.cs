namespace PaymentsClient.Domain.Payments;

public class PaymentsService : IPaymentsService
{
    private readonly INagApiClientService _nagApiClientService;

    public PaymentsService(INagApiClientService nagApiClientService)
    {
        _nagApiClientService = nagApiClientService;
    }
    
    public async Task<Result<CreatePaymentResponse>> CreatePaymentAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default)
    {
        if (!IsValidRedirectUrl(request))
        {
            return Result<CreatePaymentResponse>.Failure("Invalid redirectUrl");
        }

        if (!IsValidAmount(request))
        {
            return Result<CreatePaymentResponse>.Failure("Invalid destination amount");
        }
        
        return await _nagApiClientService.CreatePaymentAsync(request, cancellationToken);
    }

    private static bool IsValidRedirectUrl(CreatePaymentRequest request)
        => Uri.TryCreate(request.RedirectUrl, UriKind.Absolute, out Uri _);
    
    private static bool IsValidAmount(CreatePaymentRequest request)
        => request.Request.Amount.Currency is not null 
           && request.Request.Amount.Value is not null;
    
    public async Task<Result<RefreshPaymentStatusResponse>> RefreshStatusAsync(RefreshPaymentStatusRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.PaymentId))
        {
            return Result<RefreshPaymentStatusResponse>.Failure("Invalid paymentId");
        }

        return await _nagApiClientService.RefreshPaymentStatusAsync(request, cancellationToken);
    }
}