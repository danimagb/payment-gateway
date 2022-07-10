namespace PaymentGateway.Api.Handlers.ApiKeyAuthentication
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Options;

    using PaymentGateway.Api.Settings;

    using System.Security.Claims;
    using System.Text.Encodings.Web;

    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IApiKeyCacheService cacheService;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IApiKeyCacheService cacheService) 
            : base(options, logger, encoder, clock)
        {
            this.cacheService = cacheService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderName, out var apiKey) || apiKey.Count != 1)
            {
                return AuthenticateResult.Fail($"No {ApiKeyAuthenticationOptions.HeaderName} header found");
            }

            var clientId = await cacheService.GetClientIdFromApiKey(apiKey);

            if (clientId == null)
            {
                
                return AuthenticateResult.Fail("Api key is invalid");
            }



            var claims = new[] { new Claim(ClaimTypes.Name, clientId.ToString()) };
            var identity = new ClaimsIdentity(claims, ApiKeyAuthenticationOptions.DefaultScheme);
            var identities = new List<ClaimsIdentity> { identity };
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationOptions.DefaultScheme);

            return AuthenticateResult.Success(ticket);
        }
    }
}
