namespace PaymentGateway.Api.Handlers.ApiKeyAuthentication
{
    using Microsoft.Extensions.Options;

    using PaymentGateway.Api.Settings;

    public interface IClientService
    {
        Task<Dictionary<string, Guid>> GetActiveClients();
        Task InvalidateApiKey(string apiKey);
    }

    internal class InMemoryClientsService : IClientService
    {
        private readonly Dictionary<string, Guid> clients;

        public InMemoryClientsService(IOptionsMonitor<ApiKeysSettings> apiKeys)
        {
            clients = apiKeys.CurrentValue.Keys.ToDictionary(x => x.Key, x => new Guid(x.ClientId));
        }

        public Task<Dictionary<string, Guid>> GetActiveClients()
        {
            return Task.FromResult(clients);
        }

        public Task InvalidateApiKey(string apiKey)
        {
            clients.Remove(apiKey);

            return Task.CompletedTask;
        }
    }
}
