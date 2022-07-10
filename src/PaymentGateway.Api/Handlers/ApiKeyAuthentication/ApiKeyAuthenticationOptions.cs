namespace PaymentGateway.Api.Handlers.ApiKeyAuthentication
{
    using Microsoft.AspNetCore.Authentication;
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKey";
        public const string HeaderName = "x-api-key";
    }
}
