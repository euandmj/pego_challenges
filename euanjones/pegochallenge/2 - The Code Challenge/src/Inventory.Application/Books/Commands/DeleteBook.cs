using Inventory.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Books.Commands;

public static class DeleteBook
{
    public record DeleteBookRequest(long Id) : IRequest;

    internal sealed class DeleteBookHandler : IRequestHandler<DeleteBookRequest>
    {
        private readonly InventoryDbContext _context;

        public DeleteBookHandler(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.Id);

            if (book is null)
            {
                throw new BookNotFoundException(request.Id);
            }

            _context.Remove(book);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
