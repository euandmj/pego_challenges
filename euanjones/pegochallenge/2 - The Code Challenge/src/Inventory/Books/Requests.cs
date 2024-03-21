namespace Inventory.Books;

public class CreateBookRequest
{
	public string Title { get; set; } = default!;
	public string Author { get; set; } = default!;
	public DateOnly Published { get; set; }
	public int Quantity { get; set; }
}

public class UpdateBookRequest
{
	public string? Title { get; set; }
	public string? Author { get; set; }
	public DateOnly? Published { get; set; }
	public int Quantity { get; set; }
}

