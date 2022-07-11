namespace PaymentGateway.Integration.Tests
{
    using Xunit;

    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<TestServerFixture>
    {
    }

}
