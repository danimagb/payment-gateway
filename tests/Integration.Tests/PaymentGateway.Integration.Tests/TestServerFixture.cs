namespace PaymentGateway.Integration.Tests
{
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Options;

    using Npgsql;

    using PaymentGateway.Api;
    using PaymentGateway.Api.Settings;

    using Respawn;

    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public class TestServerFixture : IDisposable
    {

        public HttpClient paymentGatewayApiClient;
        public string ConnectionString;

        public TestServerFixture()
        {
            var application = new WebApplicationFactory<Program>()
           .WithWebHostBuilder(builder =>
           {
 
           });

            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            this.ConnectionString = config["ConnectionStrings:PaymentsGateway"];

            var apiKeysSettings = ((IOptions<ApiKeysSettings>)application.Services.GetService(typeof(IOptions<ApiKeysSettings>))).Value;


            this.paymentGatewayApiClient = application.CreateClient();
            this.paymentGatewayApiClient.DefaultRequestHeaders.Add("x-api-key", apiKeysSettings.Keys.FirstOrDefault()?.Key);
        }

        public void Dispose()
        {
           
        }

        public async Task CleanUpSeed()
        {
            var checkpoint = new Checkpoint
            {
                SchemasToInclude = new string[] { },
                DbAdapter = DbAdapter.Postgres
            };
            using (var conn = new NpgsqlConnection(this.ConnectionString))
            {
                await conn.OpenAsync();
                await checkpoint.Reset(conn);
            }
                
        }
    }
}