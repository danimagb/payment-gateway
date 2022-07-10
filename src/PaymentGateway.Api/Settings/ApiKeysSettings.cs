namespace PaymentGateway.Api.Settings
{
    public class ApiKeysSettings
    {
        public List<ApiKey> Keys { get; set; }
    }

    public class ApiKey
    {
        public string Key { get; set; }

        public string ClientId { get; set; }
    }
}
