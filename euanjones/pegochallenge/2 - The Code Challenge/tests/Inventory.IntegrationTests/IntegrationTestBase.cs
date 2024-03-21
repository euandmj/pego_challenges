using Inventory.IntegrationTests.Infrastructure;

namespace Inventory.IntegrationTests;

public abstract class IntegrationTestBase 
{
	private readonly string _connectionString;

	protected HttpClient HttpClient { get; }
	protected IServiceProvider Services { get; }
	protected inventory.proto3.InventoryService.InventoryServiceClient InventoryServiceClient { get; }

	protected IntegrationTestBase(InMemoryWebApplicationFactory factory)
	{
		HttpClient = factory.CreateClient();
		Services = factory.Server.Services;
		InventoryServiceClient = new inventory.proto3.InventoryService.InventoryServiceClient(Grpc.Net.Client.GrpcChannel.ForAddress("http://localhost", new Grpc.Net.Client.GrpcChannelOptions
		{
			HttpClient = HttpClient
		}));
		_connectionString = factory.ConnectionString;
	}
}

