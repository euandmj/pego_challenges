using Inventory.DataAccess;
using Inventory.DataAccess.Model;
using MediatR;

namespace Inventory.Application.Books.Commands;

public static class CreateBook
{
    public record CreateBookRequest(string Title, string Author, DateOnly Published, int Quantity) : IRequest<CreateBookResponse>;
    public record CreateBookResponse(long Id);

    internal sealed class CreateBookHandler : IRequestHandler<CreateBookRequest, CreateBookResponse>
    {
        private readonly InventoryDbContext _context;

        public CreateBookHandler(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<CreateBookResponse> Handle(CreateBookRequest request, CancellationToken cancellationToken)
        {
            var book = new BookModel
            {
                Author = request.Author,
                Title = request.Title,
                PublishDate = request.Published,
                Quantity = request.Quantity
            };

            _context.Add(book);
            await _context.SaveChangesAsync(cancellationToken);
            return new(book.Id);
        }
    }
}
