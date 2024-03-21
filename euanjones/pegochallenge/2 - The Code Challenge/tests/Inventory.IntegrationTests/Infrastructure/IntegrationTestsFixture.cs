namespace Inventory.IntegrationTests.Infrastructure;

[CollectionDefinition(Collections.IntegrationTests)]
public class IntegrationTestsFixture : ICollectionFixture<InMemoryWebApplicationFactory>;
