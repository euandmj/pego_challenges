using inventory.proto3;

namespace Inventory.IntegrationTests;

[Collection(Collections.IntegrationTests)]
public class InventoryServiceTests(InMemoryWebApplicationFactory factory) : IntegrationTestBase(factory)
{
	[Fact]
	public async Task ValidateInventory_ShouldReturnLogicalList()
	{
		var result = await InventoryServiceClient.ValidateInventoryAsync(new ValidateInventoryRequest
		{
			Items =
			{
				new Google.Protobuf.Collections.RepeatedField<InventoryItem>
				{
					new InventoryItem { Id = 2, RequiredQuantity = 5 },
				}
			}
		});
		result.Results[0].Id.Should().Be(2);
		result.Results[0].Status.Should().Be(It.IsAny<StockOption>());
	}
}

