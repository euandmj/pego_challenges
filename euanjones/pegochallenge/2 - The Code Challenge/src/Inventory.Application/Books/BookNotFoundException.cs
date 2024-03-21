namespace Inventory.Application.Books;

public class BookNotFoundException(long id) : Exception($"Book was not found with Id: {id}");