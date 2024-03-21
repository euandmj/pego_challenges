using Grpc.Core;
using inventory.proto3;

namespace Inventory.Inventory;

public class InventoryServiceImpl : InventoryService.InventoryServiceBase
{
	public override Task<ValidateInventoryResponse> ValidateInventory(ValidateInventoryRequest request, ServerCallContext context)
	{
		return Task.FromResult(new ValidateInventoryResponse
		{
			Results =
			{
				request.Items.Select(x => new InventoryValidationMessage
				{
					Id = x.Id,
					Status = Random.Shared.Next(0, 10) % 2 == 0 ? StockOption.OutOfStock : StockOption.InStock
				})
			}
		});
	}
}
