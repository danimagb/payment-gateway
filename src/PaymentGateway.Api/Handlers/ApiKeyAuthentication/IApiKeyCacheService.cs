namespace PaymentGateway.Api.Handlers.ApiKeyAuthentication
{
    using Microsoft.Extensions.Caching.Memory;
    public interface IApiKeyCacheService
    {
        ValueTask<Guid> GetClientIdFromApiKey(string apiKey);
    }

    public class ApiKeyCacheService : IApiKeyCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IClientService _clientsService;

        public ApiKeyCacheService(IMemoryCache memoryCache, IClientService clientsService)
        {
            _memoryCache = memoryCache;
            _clientsService = clientsService;
        }

        public async ValueTask<Guid> GetClientIdFromApiKey(string apiKey)
        {
            if (!_memoryCache.TryGetValue<Dictionary<string, Guid>>($"Authentication_ApiKeys", out var internalKeys))
            {
                internalKeys = await _clientsService.GetActiveClients();

                _memoryCache.Set($"Authentication_ApiKeys", internalKeys);
            }

            if (!internalKeys.TryGetValue(apiKey, out var clientId))
            {
                return Guid.Empty;
            }

            return clientId;
        }
    }
}
