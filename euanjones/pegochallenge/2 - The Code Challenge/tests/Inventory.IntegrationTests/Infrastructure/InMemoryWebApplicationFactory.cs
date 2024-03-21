using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace Inventory.IntegrationTests.Infrastructure;

public class InMemoryWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IReadOnlyCollection<IContainer> _containers;
    private readonly PostgreSqlContainer _pgsql;
    private readonly INetwork _network;

    public string ConnectionString { get; private set; } = string.Empty;

    public InMemoryWebApplicationFactory()
    {
        _network = new NetworkBuilder()
            .WithName("testcontainers")
            .WithCleanUp(true)
            .Build();

		_pgsql = new PostgreSqlBuilder()
            .WithName("inventory-db")
            .WithNetwork(_network)
            .Build();

        _containers = [_pgsql];
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }

    public async Task InitializeAsync()
    {
        await _network.CreateAsync();
		await Task.WhenAll(_containers.Select(c => c.StartAsync())).ConfigureAwait(false);
		ConnectionString = _pgsql.GetConnectionString();
	}

    async Task IAsyncLifetime.DisposeAsync()
    {
        await Task.WhenAll(_containers.Select(c => c.StopAsync())).ConfigureAwait(false);
        await _network.DeleteAsync();
    }
}

