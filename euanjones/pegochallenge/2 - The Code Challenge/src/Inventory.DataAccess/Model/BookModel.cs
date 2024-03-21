namespace Inventory.DataAccess.Model;

public class BookModel
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateOnly PublishDate { get; set; }
    public int Quantity { get; set; }
}
