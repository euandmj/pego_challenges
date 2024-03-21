using Inventory.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Books.Commands;

public static class UpdateBook
{
    public record UpdateBookRequest(long Id, string? Title, string? Author, DateOnly? Published, int? Quantity) : IRequest;

    internal sealed class UpdateBookHandler : IRequestHandler<UpdateBookRequest>
    {
        private readonly InventoryDbContext _context;

        public UpdateBookHandler(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
                ?? throw new BookNotFoundException(request.Id);

            if (request.Title is not null) book.Title = request.Title;
            if (request.Author is not null) book.Author = request.Author;
            if (request.Published is not null) book.PublishDate = request.Published.Value;
			if (request.Quantity is not null) book.Quantity = request.Quantity.Value;

			await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
