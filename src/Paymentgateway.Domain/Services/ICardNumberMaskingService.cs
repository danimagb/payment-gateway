namespace PaymentGateway.Domain.Services
{
    using PaymentGateway.Domain.ValueObjects;
    
    public interface ICardNumberMaskingService
    {
        string Mask(CardNumber number);
    }
}
