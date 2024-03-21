namespace Inventory.Books;

public class Book
{
	public long Id { get; set; }
	public string Title { get; set; } = default!;
	public string Author { get; set; } = default!;
	public DateOnly PublishDate { get; set; }
	public int Quantity { get; set; }
}